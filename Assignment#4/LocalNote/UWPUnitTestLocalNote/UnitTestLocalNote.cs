using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using LocalNote;
using System.Threading.Tasks;
using Windows.Storage;

namespace UWPUnitTestLocalNote
{
    [TestClass]
    public class UnitTest1
    {
        static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        [TestMethod]
        public async Task CheckingAvailableFilesAndCheckingReadibility()
        {
            var folder =ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                string text = await FileIO.ReadTextAsync(file);
                string title = file.Name;
                if (text == null && title == null)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.IsTrue(true);
                }
                Assert.AreEqual(0, 0);
            }
        }

        [TestMethod]
        public async Task AddFiles()
        {
            string formattitle = "text.txt";
            string content = "Contents";
            StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(formattitle,
                            CreationCollisionOption.ReplaceExisting);


            await storageFolder.GetFileAsync(formattitle);
            await FileIO.WriteTextAsync(sampleFile, content);
        }

        [TestMethod]
        public async Task EditText()
        {
            string title = "text.txt";
            string text = "";
            var folder = ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(title,
                            CreationCollisionOption.ReplaceExisting);
            await storageFolder.GetFileAsync(title);
            await FileIO.WriteTextAsync(sampleFile, text);
            StorageFile sampleFileUpdated =
                        await storageFolder.CreateFileAsync(title,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);
            string updatedText = await FileIO.ReadTextAsync(sampleFileUpdated);
            Assert.AreEqual(text, updatedText);
        }

        MainPageData mpd;
        public Note Note { get; private set; }

        [TestMethod]
        public void CheckingCreateANote()
        {
            mpd = new MainPageData();
            string title = "Test Title";
            string content = "Test Content";
            mpd.CreateNewNote(title, content);
            Assert.AreEqual(title, mpd.Notes[(mpd.Notes.IndexOf(Note)) + 1].Title, "Success write.");
        }

        [TestMethod]
        public void CheckingDeleteANote()
        {
            mpd = new MainPageData();
            string title = "Test Title";
            string content = "Test Content";
            mpd.CreateNewNote(title, content);
            Note note = mpd.Notes[(mpd.Notes.IndexOf(Note)) + 1];
            //DeleteNote(Note note)
            mpd.DeleteNote(note);
            Assert.IsTrue(!mpd.Notes.Contains(note), "Success delete.");
        }

        [TestMethod]
        public void CheckingUpdateANote()
        {
            mpd = new MainPageData();
            string title = "Test Title";
            string updatedContent = "Edited";
            mpd.CreateNewNote(title, "Not Edited");
            Note note = mpd.Notes[(mpd.Notes.IndexOf(Note)) + 1];
            //WriteToNote(Note note, string content)
            mpd.WriteToNote(note, updatedContent);
            Assert.AreEqual(updatedContent, note.Content, "Success update.");
        }
    }
}
