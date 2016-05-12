﻿using System;
using TraceableGalleryApp.Views;
using XLabs.Forms.Mvvm;
using Xamarin.Forms;
using TraceableGalleryApp.Interfaces;
using System.Collections.Generic;

namespace TraceableGalleryApp.ViewModels
{
    [ViewType(typeof(PicturePage))]
    public class PictureViewModel : BaseViewModel
    {
        readonly IPictureDatabase _pictureDatabase;
        string _addText;
        string _longitudeText;
        string _latiudeText;
        string _labelsText;
        Command _searchByLabelCommand;
        ImageSource _imgSource;
        List<string> _labels;

        public PictureViewModel(IPictureDatabase pictureDatabase)
        {
            _pictureDatabase = pictureDatabase;
        }

        public string AddText
        {
            get { return _addText; }
            set { SetObservableProperty(ref _addText, value); }
        }

        public string LongitudeText
        {
            get { return _longitudeText; }
            set { SetObservableProperty(ref _longitudeText, value); }
        }

        public string LatitudeText
        {
            get { return _latiudeText; }
            set { SetObservableProperty(ref _latiudeText, value); }
        }

        public string LabelsText
        {
            get { return _labelsText; }
            set { SetObservableProperty(ref _labelsText, value); }
        }

        public Command AddLabelCommand
        {
            get 
            { 
                return _searchByLabelCommand ?? (_searchByLabelCommand = new Command(async () => {
                    var exists = await _pictureDatabase.IsLabelExists(AddText);
                })); 
            }
        }

        public ImageSource ImgSource
        {
            get { return _imgSource; }
            set { SetObservableProperty(ref _imgSource, value); }
        }

        public List<string> Labels
        {
            get { return _labels; }
            set 
            { 
                var text = "Labels:\n";
                foreach (var label in value)
                {
                    if (value.IndexOf(label) != 0)
                        text += ", ";
                    
                    text += label;
                }

                LabelsText = text;

                SetObservableProperty(ref _labels, value); 
            }
        }
    }
}
