using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lottery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void DoTask();
        private System.Timers.Timer _timer;
        private string[] _personalPaths;
        //private List<BitmapImage> _personalImages;
        private bool _isRunning;
        private int _roundIndex;
        private List<int> _lotteryPersonals;
        private bool _isSplash;
        private bool _isShowResult;

        private Storyboard _sbSplash;
        private Storyboard _sbLottery;
        private Storyboard _sbCyShow;
        private Storyboard _sb3djShow;
        private Storyboard _sb2djShow;
        private Storyboard _sb1djShow;
        private Storyboard _sbtdjShow;

        private int _cyIndex;
        private int _3djIndex;
        private int _2djIndex;

        private MediaPlayer _mp01;
        private MediaPlayer _mp02;
        private string _01SoundPath;
        private string _02SoundPath;

        public MainWindow()
        {
            InitializeComponent();

            initSb();

            _personalPaths = PersonalManager.Instance.PersonalPaths;
            //_personalImages = new List<BitmapImage>();
            //for (var i = 0; i < _personalPaths.Length; i++)
            //{
            //    _personalImages.Add(new BitmapImage(new Uri(_personalPaths[i])));
            //}

            _lotteryPersonals = new List<int>();

            _timer = new System.Timers.Timer();
            _timer.Elapsed += timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Interval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["speed"]);

            _mp01 = new MediaPlayer();
            this._01SoundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds");
            this._01SoundPath = System.IO.Path.Combine(this._01SoundPath, "01.wav");
            _mp01.Open(new Uri(this._01SoundPath));
            _mp01.MediaEnded += _mp_MediaEnded;

            _mp02 = new MediaPlayer();
            this._02SoundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds");
            this._02SoundPath = System.IO.Path.Combine(this._02SoundPath, "02.wav");
            _mp02.Open(new Uri(this._02SoundPath));

            _isRunning = false;

            _isSplash = true;

            _isShowResult = false;

            _roundIndex = RecordManager.Instance.GetRound();
            string[] records = RecordManager.Instance.GetRecords();
            foreach (string record in records)
            {
                string[] r = record.Split('@');
                if (r.Length > 1)
                {
                    for (var i = 0; i < this._personalPaths.Length; i++)
                    {
                        if (this._personalPaths[i].Equals(r[1]))
                        {
                            this._lotteryPersonals.Add(i);
                            break;
                        }
                    }
                }
            }
        }

        void _mp_MediaEnded(object sender, EventArgs e)
        {
            _mp01.Stop();
            _mp01.Play();
        }

        private void initSb()
        {
            this._sbSplash = (Storyboard)FindResource("_sb_Splash_");
            this._sbSplash.Completed += _sbSplash_Completed;

            this._sbLottery = (Storyboard)FindResource("_sb_StartLottery_");
            this._sbLottery.Completed += _sbLottery_Completed;

            this._sbCyShow = (Storyboard)FindResource("_sb_CyShow_");
            this._sbCyShow.Completed += _sbCyShow_Completed;

            this._sb3djShow = (Storyboard)FindResource("_sb_3djShow_");
            this._sb3djShow.Completed += _sb3djShow_Completed;

            this._sb2djShow = (Storyboard)FindResource("_sb_2djShow_");
            this._sb2djShow.Completed += _sb2djShow_Completed;

            this._sb1djShow = (Storyboard)FindResource("_sb_1djShow_");
            this._sb1djShow.Completed += _sb1djShow_Completed;

            this._sbtdjShow = (Storyboard)FindResource("_sb_tdjShow_");
            this._sbtdjShow.Completed += _sbtdjShow_Completed;
        }

        void _sbtdjShow_Completed(object sender, EventArgs e)
        {
            int personal = getPersonals(true);
            BitmapImage bi = GetBitmapImage(_personalPaths[personal]);
            this._tdj1_.Source = bi;
            this._t1_.Source = bi;

            RecordManager.Instance.RecordMessage("第4轮特等奖");
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
        }

        void _sb1djShow_Completed(object sender, EventArgs e)
        {
            int personal = getPersonals(true);
            BitmapImage bi = GetBitmapImage(_personalPaths[personal]);
            this._1dj1_.Source = bi;
            this._11_.Source = bi;

            RecordManager.Instance.RecordMessage("第3轮1等奖");
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
            RecordManager.Instance.RecordMessage("");
        }

        void _sb2djShow_Completed(object sender, EventArgs e)
        {
            int personal = getPersonals(true);
            BitmapImage bi = GetBitmapImage(_personalPaths[personal]);
            switch (this._2djIndex)
            {
                case 1:
                    this._2dj1_.Source = bi;
                    this._21_.Source = bi;

                    RecordManager.Instance.RecordMessage("第2轮2等奖");
                    RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
                    break;
                case 2:
                    this._2dj2_.Source = bi;
                    this._22_.Source = bi;

                    RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
                    RecordManager.Instance.RecordMessage("");
                    break;
            }
        }

        void _sb3djShow_Completed(object sender, EventArgs e)
        {
            int personal = getPersonals(true);
            BitmapImage bi = GetBitmapImage(_personalPaths[personal]);
            switch (this._3djIndex)
            {
                case 1:
                    this._3dj1_.Source = bi;
                    this._31_.Source = bi;

                    RecordManager.Instance.RecordMessage("第1轮3等奖");
                    RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
                    break;
                case 2:
                    this._3dj2_.Source = bi;
                    this._32_.Source = bi;

                    RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
                    break;
                case 3:
                    this._3dj3_.Source = bi;
                    this._33_.Source = bi;

                    RecordManager.Instance.RecordMessage("@" + _personalPaths[personal]);
                    RecordManager.Instance.RecordMessage("");
                    break;
            }
        }

        void _sbCyShow_Completed(object sender, EventArgs e)
        {
            int[] personals = new int[5];
            personals[0] = getPersonals(true);
            personals[1] = getPersonals(true);
            personals[2] = getPersonals(true);
            personals[3] = getPersonals(true);
            personals[4] = getPersonals(true);


            BitmapImage[] personalImgs = new BitmapImage[5];
            personalImgs[0] = GetBitmapImage(_personalPaths[personals[0]]);
            this._canyu1_.Source = personalImgs[0];

            personalImgs[1] = GetBitmapImage(_personalPaths[personals[1]]);
            this._canyu2_.Source = personalImgs[1];

            personalImgs[2] = GetBitmapImage(_personalPaths[personals[2]]);
            this._canyu3_.Source = personalImgs[2];

            personalImgs[3] = GetBitmapImage(_personalPaths[personals[3]]);
            this._canyu4_.Source = personalImgs[3];

            personalImgs[4] = GetBitmapImage(_personalPaths[personals[4]]);
            this._canyu5_.Source = personalImgs[4];

            switch (_cyIndex)
            {
                case 1:
                    this._cy11_.Source = personalImgs[0];
                    this._cy12_.Source = personalImgs[1];
                    this._cy13_.Source = personalImgs[2];
                    this._cy14_.Source = personalImgs[3];
                    this._cy15_.Source = personalImgs[4];
                    RecordManager.Instance.RecordMessage("第1轮参与奖");
                    break;
                case 2:
                    this._cy21_.Source = personalImgs[0];
                    this._cy22_.Source = personalImgs[1];
                    this._cy23_.Source = personalImgs[2];
                    this._cy24_.Source = personalImgs[3];
                    this._cy25_.Source = personalImgs[4];
                    RecordManager.Instance.RecordMessage("第2轮参与奖");
                    break;
                case 3:
                    this._cy31_.Source = personalImgs[0];
                    this._cy32_.Source = personalImgs[1];
                    this._cy33_.Source = personalImgs[2];
                    this._cy34_.Source = personalImgs[3];
                    this._cy35_.Source = personalImgs[4];
                    RecordManager.Instance.RecordMessage("第3轮参与奖");
                    break;
                case 4:
                    this._cy41_.Source = personalImgs[0];
                    this._cy42_.Source = personalImgs[1];
                    this._cy43_.Source = personalImgs[2];
                    this._cy44_.Source = personalImgs[3];
                    this._cy45_.Source = personalImgs[4];
                    RecordManager.Instance.RecordMessage("第4轮参与奖");
                    break;
            }

            RecordManager.Instance.RecordMessage("@" + _personalPaths[personals[0]]);
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personals[1]]);
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personals[2]]);
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personals[3]]);
            RecordManager.Instance.RecordMessage("@" + _personalPaths[personals[4]]);
            RecordManager.Instance.RecordMessage("");
        }

        void _sbSplash_Completed(object sender, EventArgs e)
        {
            this._splash_.Visibility = System.Windows.Visibility.Collapsed;
        }

        void _sbLottery_Completed(object sender, EventArgs e)
        {
            //_mp01.Stop();
            //doPlay(this._01SoundPath);
            _timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            if (_isRunning)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new DoTask(DoMyTask));
            }
            else
            {
                _mp01.Stop();
            }
        }

        private void DoMyTask()
        {
            int index = getPersonals(false);
            if (index < _personalPaths.Length)
            {
                //_personalContainer_.Source = _personalImages[index];

                //_personalContainer_.Source = new BitmapImage(new Uri(_personalPaths[index]));
                _personalContainer_.Source = GetBitmapImage(_personalPaths[index]);

            }

            _timer.Start();
        }

        public static BitmapImage GetBitmapImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(File.ReadAllBytes(path));
                bitmap.EndInit();
            }
            finally
            {
                bitmap.Freeze();
            }
            return bitmap;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (this._isShowResult)
                {
                    if (this._roundIndex < 11)
                    {
                        this._canyuContainer_.Visibility = System.Windows.Visibility.Collapsed;
                        this._3djContainer_.Visibility = System.Windows.Visibility.Collapsed;
                        this._2djContainer_.Visibility = System.Windows.Visibility.Collapsed;
                        this._1djContainer_.Visibility = System.Windows.Visibility.Collapsed;
                        this._tdjContainer_.Visibility = System.Windows.Visibility.Collapsed;

                        _personalContainer_.Source = null;

                        this._isShowResult = false;
                    }
                }
                else
                {
                    doLottery();
                }
            }
        }

        private void doLottery()
        {
            if (_isSplash)
            {
                doSplashShow();
                this._isSplash = false;
            }
            else
            {
                if (_roundIndex > 10) { return; }

                _isRunning = !_isRunning;
                if (_isRunning)
                {
                    doLotteryShow();
                }
                else
                {
                    _mp01.Stop();

                    switch (_roundIndex)
                    {
                        case 0: // 第1轮参与奖
                            doCYShow(1);
                            break;
                        case 1: // 第1轮 3等奖 1
                            do3DJShow(1);
                            break;
                        case 2: // 第1轮 3等奖 2
                            do3DJShow(2);
                            break;
                        case 3: // 第1轮 3等奖 3
                            do3DJShow(3);
                            break;
                        case 4: // 第2轮参与奖
                            doCYShow(2);
                            break;
                        case 5: // 第2轮 2等奖 1
                            do2DJShow(1);
                            break;
                        case 6: // 第2轮 2等奖 2
                            do2DJShow(2);
                            break;
                        case 7: // 第3轮参与奖
                            doCYShow(3);
                            break;
                        case 8: // 第3轮 1等奖
                            do1DJShow();
                            break;
                        case 9: // 第4轮参与奖
                            doCYShow(4);
                            break;
                        case 10: // 第4轮 特等奖
                            doTDJShow();
                            break;

                    }
                    _roundIndex++;
                    RecordManager.Instance.RecordRound(_roundIndex);
                }
            }
        }

        private int getPersonals(bool isLottery)
        {
            Random r = new Random();
            bool again = true;
            int index;
            do
            {
                index = r.Next(_personalPaths.Length);
                if (!_lotteryPersonals.Contains(index))
                {
                    again = false;
                }
            } while (again);

            if (isLottery)
            {
                _lotteryPersonals.Add(index);
            }

            return index;
        }

        private void doSplashShow()
        {
            this._sbSplash.Begin();
        }

        private void doLotteryShow()
        {
            _mp01.Stop();
            _mp01.Play();
            //doPlay(this._01SoundPath);
            this._sbLottery.Begin();
        }

        // 参与奖
        private void doCYShow(int index)
        {
            this._canyu1_.Source = null;
            this._canyu2_.Source = null;
            this._canyu3_.Source = null;
            this._canyu4_.Source = null;
            this._canyu5_.Source = null;

            this._cyIndex = index;
            this._canyuContainer_.Visibility = System.Windows.Visibility.Visible;
            this._sbCyShow.Begin();

            this._isShowResult = true;

            _mp02.Stop();
            _mp02.Play();
            //doPlay(this._02SoundPath);
        }

        // 3等奖
        private void do3DJShow(int index)
        {
            this._3djIndex = index;
            this._3djContainer_.Visibility = System.Windows.Visibility.Visible;
            this._sb3djShow.Begin();

            this._isShowResult = true;

            //doPlay(this._02SoundPath);
            _mp02.Stop();
            _mp02.Play();
        }

        // 2等奖
        private void do2DJShow(int index)
        {
            this._2djIndex = index;
            this._2djContainer_.Visibility = System.Windows.Visibility.Visible;
            this._sb2djShow.Begin();

            this._isShowResult = true;

            //doPlay(this._02SoundPath);
            _mp02.Stop();
            _mp02.Play();
        }

        // 1等奖
        private void do1DJShow()
        {
            this._1djContainer_.Visibility = System.Windows.Visibility.Visible;
            this._sb1djShow.Begin();

            this._isShowResult = true;

            //doPlay(this._02SoundPath);
            _mp02.Stop();
            _mp02.Play();
        }

        // 特等奖
        private void doTDJShow()
        {
            this._tdjContainer_.Visibility = System.Windows.Visibility.Visible;
            this._sbtdjShow.Begin();



            this._isShowResult = true;

            //doPlay(this._02SoundPath);
            _mp02.Stop();
            _mp02.Play();
        }

        //private void doPlay(string name)
        //{
        //    _mp01.Stop();
        //    try
        //    {
        //        _mp01.Open(new Uri(name));
        //        _mp01.Play();
        //    }
        //    catch { }
        //}

        //private void stop01()
        //{
        //    _mp01.Stop();
        //}

        //private void doPlay02()
        //{
        //    try
        //    {
        //        _mp02.Open(new Uri(this._02SoundPath));
        //        _mp02.Play();
        //    }
        //    catch { }
        //}

        //private void stop02()
        //{
        //    _mp02.Stop();
        //}
    }
}
