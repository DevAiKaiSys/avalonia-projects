using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using BatchProcess3.ViewModels;

namespace BatchProcess3.Services;

public class PrinterService
{
    public ObservableCollection<PrintersViewModel> AvailablePrinters()
    {
        var printers = new ObservableCollection<PrintersViewModel>();

        /*printers.Add(new PrinterDetailsViewModel { Id = "0", Name = "(Default)" });*/
        /*var defaultPrinter = new PrintersViewModel
        {
            Id = "0",
            Name = "(Default)"
        };
        defaultPrinter.PaperSizes.Add(new KeyValuePair<string, string>("0", "(Default)"));
        defaultPrinter.SourceTrays.Add(new KeyValuePair<string, string>("0", "(Default)"));
        printers.Add(defaultPrinter);

        var index = 1;*/
        printers.Add(new PrintersViewModel { Id = "(Default)", Name = "(Default)" });

        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var printDocument = new PrintDocument();

            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                /*printers.Add(new PrinterDetailsViewModel { Id = index.ToString(), Name = printerName });
                index++;*/
                /*var printerDetailsViewModel = new PrintersViewModel { Id = index.ToString(), Name = printerName };*/
                var printerDetailsViewModel = new PrintersViewModel { Id = printerName, Name = printerName };

                printDocument.PrinterSettings.PrinterName = printerName;

                // Add Default option
                /*printerDetailsViewModel.PaperSizes.Add(new KeyValuePair<string, string>("(Default)", "(Default)"));*/
                printerDetailsViewModel.PaperSizes.Add("(Default)");

                /*var paperSizeIndex = 1;
                foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes)
                {
                    printerDetailsViewModel.PaperSizes.Add(
                        new KeyValuePair<string, string>(paperSizeIndex.ToString(), paperSize.PaperName));
                    paperSizeIndex++;
                }*/
                foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes)
                    /*printerDetailsViewModel.PaperSizes.Add(
                        new KeyValuePair<string, string>(paperSize.PaperName, paperSize.PaperName));*/
                    printerDetailsViewModel.PaperSizes.Add(paperSize.PaperName);

                // Add Default option
                /*printerDetailsViewModel.SourceTrays.Add(new KeyValuePair<string, string>("(Default)", "(Default)"));*/
                printerDetailsViewModel.SourceTrays.Add("(Default)");

                /*var sourceTrayIndex = 1;
                foreach (PaperSource sourceTray in printDocument.PrinterSettings.PaperSources)
                {
                    printerDetailsViewModel.SourceTrays.Add(
                        new KeyValuePair<string, string>(sourceTrayIndex.ToString(), sourceTray.SourceName));
                    sourceTrayIndex++;
                }*/
                foreach (PaperSource sourceTray in printDocument.PrinterSettings.PaperSources)
                    /*printerDetailsViewModel.SourceTrays.Add(
                        new KeyValuePair<string, string>(sourceTray.SourceName, sourceTray.SourceName));*/
                    printerDetailsViewModel.SourceTrays.Add(sourceTray.SourceName);

                printers.Add(printerDetailsViewModel);
                /*index++;*/
            }
        }

        return printers;
    }
}