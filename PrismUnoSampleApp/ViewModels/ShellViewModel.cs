﻿using Prism.Events;
using Prism.Mvvm;
using PrismUnoSampleApp.Infrastructures.Events;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ReadOnlyReactivePropertySlim<string> StatusbarMessage { get; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            StatusbarMessage = _eventAggregator.GetEvent<UpdateGlobalMessageEvent>()
                .ToObservable()
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim();
        }
    }
}
