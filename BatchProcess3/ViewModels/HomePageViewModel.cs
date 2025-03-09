using BatchProcess3.Data;

namespace BatchProcess3.ViewModels;

/*public class HomePageViewModel : PageViewModel
{
    public HomePageViewModel()
    {
        PageName = ApplicationPageNames.Home;
    }
}*/
public class HomePageViewModel() : PageViewModel(ApplicationPageNames.Home)
{
    public string Test { get; set; } = "Home";
}