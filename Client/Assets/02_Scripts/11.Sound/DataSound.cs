using UnityEngine;
using UnityEngine.UIElements;

public class DataSound : VisualElement {
    public int index { get; set; }

    public new class UxmlFactory : UxmlFactory<DataSound, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits {
        UxmlIntAttributeDescription i_index = new UxmlIntAttributeDescription {
            name = "Index",
            defaultValue = 0
        };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // 텍스트 파일의 값들을 읽어 올께
            base.Init(ve, bag, cc);

            var dve = ve as DataSound;

            dve.index = i_index.GetValueFromBag(bag, cc);
        }
    }
}
