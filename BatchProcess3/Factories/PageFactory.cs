using System;
using BatchProcess3.Data;
using BatchProcess3.ViewModels;

namespace BatchProcess3.Factories;

public class PageFactory(Func<ApplicationPageNames, PageViewModel> factory)
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
    {
        return factory.Invoke(pageName);
    }
}