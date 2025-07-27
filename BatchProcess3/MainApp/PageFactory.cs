using System;
using BatchProcess3.ViewModels;

namespace BatchProcess3.MainApp;

public class PageFactory(Func<ApplicationPageNames, PageViewModel> factory)
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
    {
        return factory.Invoke(pageName);
    }
}