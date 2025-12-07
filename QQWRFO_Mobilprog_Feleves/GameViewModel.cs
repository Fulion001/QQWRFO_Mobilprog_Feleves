using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

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
    internal class GameViewModel
    {
    }
}
