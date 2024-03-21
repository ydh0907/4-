using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace UnityToolbarExtender {
    public static class ToolbarCallback { // static 클래스의 모든 맴버는 static으로 선언되어야 한다
        // Editor의 Assembly에서 "~~"이름을 가진 Assembly를 가져와라 
        static Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        static Type m_guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");

#if UNITY_2020_1_OR_NEWER // #if : 조건을 달면 그에 따른 조건에 따라 실행된다 
        static Type m_iWindowBackendType = typeof(Editor).Assembly.GetType("UnityEditor.IWindowBackend");

        // PropertyInfo : Property타입의 데이터를 가져와라
        // "~~"이름의 Property를 가져올건데 BindingFlags가 public or nonPublic or Instance인 애만 가져오겠다
        static PropertyInfo m_windowBackend = m_guiViewType.GetProperty("windowBackend",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        static PropertyInfo m_viewVisualTree = m_iWindowBackendType.GetProperty("visualTree",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

#else // #else : 위의 if조건에 해당되지 않다면 아래의 코드가 실행된다
		static PropertyInfo m_viewVisualTree = m_guiViewType.GetProperty("visualTree",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

#endif // 끝났다면 endif로 닫아주기

        static FieldInfo m_imguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        static ScriptableObject m_currentToolbar;

        /// <summary>
        /// Callback for toolbar OnGUI method.
        /// </summary>
        public static Action OnToolbarGUI;
        public static Action OnToolbarGUILeft;
        public static Action OnToolbarGUIRight;

        static ToolbarCallback() {  // unity 창의 업데이트 된 화면을 다시 그려준다. 프레임당 여러번 호출 될 수 있음
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        public static VisualElement RootVisualElement;

        static void OnUpdate() {
            // Relying on the fact that toolbar is ScriptableObject and gets deleted when layout changes
            if (m_currentToolbar == null) {
                // Find toolbar
                var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);

                m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
                if (m_currentToolbar != null) {
#if UNITY_2021_1_OR_NEWER
                    var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                    var rawRoot = root.GetValue(m_currentToolbar);
                    var mRoot = rawRoot as VisualElement;

                    RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                    RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);

                    void RegisterCallback(string root, Action cb) {
                        var toolbarZone = mRoot.Q(root);

                        var parent = new VisualElement() {
                            style = {
                                flexGrow = 1,
                                flexDirection = FlexDirection.RowReverse,
                            }
                        };

                        var container = new IMGUIContainer();
                        container.style.flexGrow = 1;
                        container.onGUIHandler += () => {
                            cb?.Invoke();
                        };

                        parent.Add(container);
                        toolbarZone.Add(parent);

                        IMGUIEvent evt = new IMGUIEvent();
                        evt.target = parent;
                    }
#else
#if UNITY_2020_1_OR_NEWER
                    var windowBackend = m_windowBackend.GetValue(m_currentToolbar);

					// Get it's visual tree
					var visualTree = (VisualElement) m_viewVisualTree.GetValue(windowBackend, null);
#else
					// Get it's visual tree
					var visualTree = (VisualElement) m_viewVisualTree.GetValue(m_currentToolbar, null);
#endif

					// Get first child which 'happens' to be toolbar IMGUIContainer
					var container = (IMGUIContainer) visualTree[0];

					// (Re)attach handler
					var handler = (Action) m_imguiContainerOnGui.GetValue(container);
					handler -= OnGUI;
					handler += OnGUI;
					m_imguiContainerOnGui.SetValue(container, handler);

                    RootVisualElement = container;
#endif
                }
            }
        }

        static void OnGUI() {
            var handler = OnToolbarGUI;
            handler?.Invoke();
        }
    }
}