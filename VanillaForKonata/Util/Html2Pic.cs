using PuppeteerSharp;

namespace VanillaForKonata.Util {
    public static class HTML2Pic {
        public static async Task<bool> generate(string webPagePath,string outputFileName,int wid=1920,int hei=1080) {

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            })) 
            {
                using (var page = await browser.NewPageAsync())
                {
                    await page.SetViewportAsync(new ViewPortOptions
                    {
                        Width = wid,
                        Height = hei
                    });
                    await page.GoToAsync(webPagePath);
                    await page.ScreenshotAsync(outputFileName);
                    return true;
                }
                
                
            }

                
            

        }
        
    
    }


}