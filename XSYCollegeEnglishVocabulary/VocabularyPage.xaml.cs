using XSYCollegeEnglishVocabulary.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XSYCollegeEnglishVocabulary.DataModel;
using Windows.Media.Playback;
using System.Threading.Tasks;
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace XSYCollegeEnglishVocabulary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VocabularyPage : Page
    {
        private MediaPlayer _mediaPlayer;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public VocabularyPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public string UnitId { get; set; }
        public string LearningBook { get; set; }
        public string[] Book_Unit_Ids { get; set; }

        const string UNIT_NAME = "第{0}册 第{1}单元";

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // unitId:"1_1"
            string unitId = (string)e.NavigationParameter;
            var book_unit_ids = unitId.Split('_');
            Book_Unit_Ids = book_unit_ids;
            UnitId = unitId;

            
            var unitA = await VocabularyDataSource.GetVocabulariesAsync(unitId: unitId, bookText: "A");

            this.DefaultViewModel["Vocabularies"] = unitA.Vocabularies;
            this.LearningBook = "A";
            this.DefaultViewModel["UnitName"] = string.Format(UNIT_NAME, book_unit_ids[0], book_unit_ids[1]);

            this.DefaultViewModel["BookText"] = "词汇A";
            this.DefaultViewModel["SwitchingBookText"] = "词汇B";
        
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // initialize the backgroundmediaplayer intanse on purpose.
            _mediaPlayer = BackgroundMediaPlayer.Current;
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ViewAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            PivotItem pivotItem = (PivotItem)pivot.SelectedItem;
            var flipView = pivotItem.Content as FlipView;
            if (flipView != null)
            {
                var word = flipView.SelectedItem as Vocabulary;
                word.DefinitionVisible = !word.DefinitionVisible;

                ViewAppBarButton.Label = word.DefinitionVisible ? "隐藏释义" : "显示释义";
            }
        }


        private async void PlayAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            PivotItem pivotItem = (PivotItem)pivot.SelectedItem;
            var flipView = pivotItem.Content as FlipView;
            if (flipView != null)
            {
                var word = flipView.SelectedItem as Vocabulary;
                var voiceFile = Constants.DATA_BASE_PATH + "horizonread" + Book_Unit_Ids[0]+"/" + word.Voice;
                voiceFile = voiceFile.Replace("\\", "/");
                await ReadVoice(voiceFile);
            }
        }

        private static async Task ReadVoice(string voiceFile)
        {
            if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
            {
                BackgroundMediaPlayer.Current.Pause();
            }
            else if (MediaPlayerState.Paused == BackgroundMediaPlayer.Current.CurrentState ||
                     MediaPlayerState.Closed == BackgroundMediaPlayer.Current.CurrentState)
            {
                if (!string.IsNullOrEmpty(voiceFile))
                {
                    string[] fileInfo = new[] { "vocabulary", voiceFile };
                    var message = new ValueSet
                    {
                        {
                            "Play",
                            fileInfo
                        }
                    };
                    BackgroundMediaPlayer.SendMessageToBackground(message);
                }
                else
                {
                    MessageDialog md2 = new MessageDialog("No file to play!", "audio");
                    await md2.ShowAsync();
                }
            }
        }

        private async void SwitchLesson_Click(object sender, RoutedEventArgs e)
        {
            if (this.LearningBook == "A")
            {
                this.LearningBook = "B";
                this.DefaultViewModel["BookText"] = "词汇B";
                this.DefaultViewModel["SwitchingLessonName"] = "词汇A";

                var unitB = await VocabularyDataSource.GetVocabulariesAsync(unitId: UnitId, bookText: "B");
                this.DefaultViewModel["Vocabularies"] = unitB.Vocabularies;
            }
            else
            {
                this.LearningBook = "A";
                this.DefaultViewModel["BookText"] = "词汇A";
                this.DefaultViewModel["SwitchingLessonName"] = "词汇B";

                var unitA = await VocabularyDataSource.GetVocabulariesAsync(unitId: UnitId, bookText: "A");
                this.DefaultViewModel["Vocabularies"] = unitA.Vocabularies;
            }

            
        }
    }
}
