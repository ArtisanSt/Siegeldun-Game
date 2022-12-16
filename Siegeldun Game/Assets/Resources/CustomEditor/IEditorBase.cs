public interface IEditorBase
{
    System.Type scriptType { get; }
    void OnInspectorGUI();
    void HeaderSettings();
    void BodySettings();
    void FooterSettings();
}
