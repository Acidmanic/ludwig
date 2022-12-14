namespace Ludwig.Common.Document
{
    public interface IDocument
    {
        IDocument Text(string text);


        IDocument BeginItalic();
        IDocument EndItalic();

        IDocument BeginBold();
        IDocument EndBold();


        IDocument BeginTitle();
        IDocument EndTitle();

        IDocument BeginSubTitle();
        IDocument EndSubTitle();

        IDocument NewLine();

        IDocument BeginLink(string src);
        IDocument EndLink();


        IDocument BeginBulletList();
        IDocument EndBulletList();

        IDocument BeginNumberedList();
        IDocument EndNumberedList();

        IDocument Image(string src, string alt = null);
        
        
    }
}