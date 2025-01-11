
namespace Wpf_AuthorsBooksEFCoreDBFirst.Services
{
    public interface IWindowService
    {
        bool ShowAuthorDialog(ref string authorName);
        bool ShowBookDialog(ref string bookTitle);
    }
}
