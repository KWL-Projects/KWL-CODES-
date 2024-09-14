namespace KWLCodes_HMSProject.Maui;

public partial class DownloadMarksPage : ContentPage
{
	public DownloadMarksPage()
	{
		InitializeComponent();
	}

    private async void OnDownloadMarksClicked(object sender, EventArgs e)
    {
        var marks = GetMarks();
        string csvContent = GenerateCsv(marks);

        string filePath = Path.Combine(FileSystem.CacheDirectory, "Marks.csv");
        File.WriteAllText(filePath, csvContent);

        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Download Marks",
            File = new ShareFile(filePath)
        });

        await DisplayAlert("Success", "Marks downloaded successfully.", "OK");
    }

}