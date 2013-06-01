﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ExampleLibrary;
using OxyPlot;

namespace ExampleBrowser
{
    using System.Reflection;

    using OxyPlot.Metro;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public bool MeasureFrameRate { get; set; }

        private double frameRate;

        public double FrameRate
        {
            get
            {
                return this.frameRate;
            }
            set
            {
                this.frameRate = value; RaisePropertyChanged("FrameRate");
            }
        }

        private IEnumerable<ExampleInfo> examples;
        public IEnumerable<ExampleInfo> Examples
        {
            get { return examples; }
            set { examples = value; RaisePropertyChanged("Examples"); }
        }

        public ICollectionView ExamplesView { get; set; }

        private ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set
            {
                selectedExample = value;
                RaisePropertyChanged("SelectedExample");
                RaisePropertyChanged("PlotBackground");
            }
        }

        public Brush PlotBackground
        {
            get
            {
                return selectedExample != null && selectedExample.PlotModel.Background != null ? selectedExample.PlotModel.Background.ToBrush() : new SolidColorBrush() { Opacity = 0 }; // Brushes.Transparent;
            }
        }

        public string Version
        {
            get
            {
                return typeof(PlotModel).GetTypeInfo().Assembly.FullName.Split(',')[1];
            }
        }

        public MainPageViewModel()
        {
            Examples = ExampleLibrary.Examples.GetList();

            var groups =
                from example in Examples
                group example by example.Category into g
                orderby g.Key
                select g;

            var ex = new CollectionViewSource() { Source = groups, IsSourceGrouped = true };
            ExamplesView = ex.View;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}