using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel; // Vagy egyéni ObservableObject implementáció
using CommunityToolkit.Mvvm.Input;

namespace QQWRFO_Mobilprog_Feleves
{
    public class PuzzlePiece : ObservableObject
    {
        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public int OriginalIndex { get; set; } // Helyes pozíció 0-tól N-ig
        public int CurrentIndex { get; set; }  // Aktuális pozíció
    }
    public class GameViewModel : ObservableObject
    {
        private readonly int _boardSize = 3; // 3x3-as kirakós
        private const double ShakeThreshold = 10.0; // Gyorsulás küszöb (g-ben)

        // UI-hoz kötött gyűjtemény
        public ObservableCollection<PuzzlePiece> PuzzlePieces { get; private set; }

        private ImageSource _selectedImage;
        // A kiválasztott/elkészített teljes kép előnézete
        public ImageSource SelectedImage
        {
            get => _selectedImage;
            private set => SetProperty(ref _selectedImage, value);
        }

        // A tábla mérete (a XAML GridItemsLayout Span tulajdonságához)
        public int BoardSize => _boardSize;

        private bool _isGameStarted;
        public bool IsGameStarted
        {
            get => _isGameStarted;
            private set => SetProperty(ref _isGameStarted, value);
        }

        // --- Parancsok (Command) ---
        public ICommand PickImageCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand PieceTappedCommand { get; }
        public ICommand ShowOriginalImageCommand { get; } // Bár a XAML-ben nem volt rá parancs, hozzáadom

        public GameViewModel()
        {
            PuzzlePieces = new ObservableCollection<PuzzlePiece>();

            // Parancsok inicializálása
            PickImageCommand = new AsyncRelayCommand(PickImageAsync);
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            StartGameCommand = new RelayCommand(StartGame);
            PieceTappedCommand = new RelayCommand<PuzzlePiece>(HandlePieceTapped);
            // ...

            // Gyorsulásmérő inicializálása a rázás figyeléséhez
            SetupAccelerometer();
        }

        // --- Képkezelés ---

        private async Task PickImageAsync()
        {
            try
            {
                // MAUI FilePicker használata kép kiválasztásához
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Válassz képet",
                    // Pl. csak JPG és PNG engedélyezése
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.image" } },
                        { DevicePlatform.Android, new[] { "image/*" } },
                        { DevicePlatform.WinUI, new[] { ".jpg", ".png" } },
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                    })
                });

                if (result != null)
                {
                    // A kiválasztott fájl elérési útjának beolvasása, és ImageSource beállítása
                    SelectedImage = ImageSource.FromFile(result.FullPath);
                    IsGameStarted = false; // Új kép választásakor visszaállítás
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Hiba a kép kiválasztásakor: {ex.Message}");
                // UI értesítés megjelenítése
            }
        }

        private async Task TakePhotoAsync()
        {
            // A MediaPicker használata kép készítéséhez (ellenőrizd a jogosultságokat!)
            if (MediaPicker.IsCaptureSupported)
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    SelectedImage = ImageSource.FromFile(photo.FullPath);
                    IsGameStarted = false;
                }
            }
            else
            {
                Debug.WriteLine("Kamera nem támogatott ezen az eszközön.");
                // UI értesítés megjelenítése
            }
        }

        // --- Játéklogika ---

        private void StartGame()
        {
            if (SelectedImage == null)
            {
                // UI értesítés: Válassz először képet!
                return;
            }

            // 1. Kép felosztása (Itt kellene a SkiaSharp vagy egyéni logikával felosztani a SelectedImage-et)
            // Jelenleg egy helyettesítő logikát használunk, ami létrehoz 9 (3x3) PuzzlePiece-t

            // Jelenleg: Placeholder darabok létrehozása (ideális esetben vágott képek lennének)
            PuzzlePieces.Clear();
            for (int i = 0; i < _boardSize * _boardSize; i++)
            {
                // A tényleges logikának a vágott képeket kellene beállítania!
                PuzzlePieces.Add(new PuzzlePiece
                {
                    OriginalIndex = i,
                    CurrentIndex = i,
                    ImageSource = SelectedImage // Helyettesítő: az egész kép látszik
                });
            }

            // 2. Keverés indítása
            ShufflePieces();
            IsGameStarted = true;
        }

        private void ShufflePieces()
        {
            // Implementáld a kirakós darabok keverését (pl. Fisher-Yates shuffle)
            // Ügyelj arra, hogy a 15-ös kirakósnál páros inversziós számmal keverj a megoldhatóság érdekében (Slide Puzzle)

            Random random = new Random();
            int n = PuzzlePieces.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                // Cserélje fel a PuzzlePieces[k]-t és a PuzzlePieces[n]-t
                (PuzzlePieces[n].CurrentIndex, PuzzlePieces[k].CurrentIndex) = (PuzzlePieces[k].CurrentIndex, PuzzlePieces[n].CurrentIndex);

                // Megjegyzés: A CollectionView frissítéséhez az ImageSource-t a ViewModel-ben ki kell cserélni
                // Ideális esetben a PuzzlePieces gyűjtemény a darabokat tárolja, és a darabok pozíciójának cseréje
                // (ami az ImageSource tulajdonság cseréjét jelenti) oldja meg a kirakós vizuális keverését.
                // A tényleges Slide Puzzle-ben a darabok pozíciójában lévő "üres" darabbal cserélnek.
            }

            // Az üres mező beállítása (általában a tábla utolsó darabja)
            // A kirakós darabok utolsó (üres) darabjának ImageSource-ját Null-ra/átlátszóra kell állítani.
            // PuzzlePieces.Last().ImageSource = null;
        }

        private void HandlePieceTapped(PuzzlePiece tappedPiece)
        {
            if (!IsGameStarted) return;

            // Logika a darab mozgatására (ellenőrizd, hogy a megérintett darab szomszédos-e az üres mezővel)
            // Ha mozgatható, cseréld fel a tappedPiece.ImageSource-t és az üres mező ImageSource-ját.

            Debug.WriteLine($"Darab megérintve: Eredeti index: {tappedPiece.OriginalIndex}, Aktuális index: {tappedPiece.CurrentIndex}");

            // A kirakós darabok vizuális cseréje (az ImageSource-ok felcserélése a gyűjteményben)
            // ...

            CheckForWin();
        }

        private void CheckForWin()
        {
            // Ellenőrizd, hogy minden darab a helyes (OriginalIndex == CurrentIndex) pozícióban van-e.
            // Ha igen: UI értesítés megjelenítése: Nyertél!
        }

        // --- Rázás Érzékelés (Shake Gesture) ---

        private void SetupAccelerometer()
        {
            if (Accelerometer.IsSupported)
            {
                // Rázás csak akkor legyen aktív, ha a játék elindult
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            // Gyorsulásmérés leállítása, ha a játék nem fut, vagy nincs kép
            if (!IsGameStarted || SelectedImage == null)
            {
                if (Accelerometer.IsMonitoring) Accelerometer.Stop();
                return;
            }

            // Kezdd el a figyelést, ha még nem fut
            if (!Accelerometer.IsMonitoring) Accelerometer.Start(SensorSpeed.Game);


            var data = e.Reading;

            // A rázás érzékelése a gyorsulás vektor nagyságának változásával történik
            double acceleration = Math.Sqrt(data.Acceleration.X * data.Acceleration.X +
                                            data.Acceleration.Y * data.Acceleration.Y +
                                            data.Acceleration.Z * data.Acceleration.Z);

            // Ha a gyorsulás meghaladja a küszöböt, keverjük újra a darabokat
            if (acceleration > ShakeThreshold)
            {
                // Keverd a darabokat!
                Debug.WriteLine($"Rázás érzékelve! Gyorsulás: {acceleration}");
                ShufflePieces();
            }
        }

        // FONTOS: Az alkalmazás bezárásakor/lap elhagyásakor le kell állítani a gyorsulásmérőt!
        public void OnDisappearing()
        {
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
            }
        }
    }
}
