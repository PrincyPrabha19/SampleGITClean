using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
	public partial class NotifierView
    {
		#region Const
		private const double MIN_LABEL_WIDTH = 400;
		private const double MAX_LABEL_WIDTH = 600;
        private const double MAX_LABEL_HEIGHT = 600;
        private const double DELTA_HEIGHT = 127;
		private const double DELTA_WIDTH = 100;
        #endregion

        #region Properties
        private NotifierResult buttonClicked = NotifierResult.None;
        public NotifierResult ButtonClicked => buttonClicked;

        private int customButtonClicked = (int)NotifierResult.None;
        public int CustomButtonClicked => customButtonClicked;

        private NotifierButtons notifierButtons = NotifierButtons.Ok;

        private static readonly DependencyProperty doNotShowAgainVisibilityProperty = DependencyProperty.Register("DoNotShowAgainVisibility", typeof(Visibility), typeof(NotifierView), new UIPropertyMetadata(Visibility.Collapsed));
        public Visibility DoNotShowAgainVisibility
        {
            get { return (Visibility)GetValue(doNotShowAgainVisibilityProperty); }
            set { SetValue(doNotShowAgainVisibilityProperty, value); }
        }
        #endregion

        #region Constructor
        public NotifierView()
		{
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();
			defaultInitialization();
		}

	    public NotifierView(string title) : this()
        {
            Title = title;
        }

        public NotifierView(string title, string message) : this(title)
        {
            var txt = new FormattedText(message, CultureInfo.CurrentCulture, textboxMessage.FlowDirection,
                new Typeface(textboxMessage.FontFamily, textboxMessage.FontStyle, textboxMessage.FontWeight, textboxMessage.FontStretch), 
                textboxMessage.FontSize, textboxMessage.Foreground);

            if (txt.Width <= MIN_LABEL_WIDTH)
                Width = MIN_LABEL_WIDTH + DELTA_WIDTH;
            else if (txt.Width <= MAX_LABEL_WIDTH)
                Width = txt.Width + DELTA_WIDTH;
            else
                txt.MaxTextWidth = MAX_LABEL_WIDTH;

            if (txt.Height <= MAX_LABEL_HEIGHT)
                Height = txt.Height + DELTA_HEIGHT;
            else
                Height = MAX_LABEL_HEIGHT + DELTA_HEIGHT;
            
            textboxMessage.Text = message;
        }

        public NotifierView(string title, string message, NotifierIcon icon) : this(title, message)
        {
            switch (icon)
            {
                case NotifierIcon.Warning:
                    imageIcon.Source = IconLoader.Warning;
                    break;
                case NotifierIcon.Information:
                    imageIcon.Source = IconLoader.Information;
                    break;
                case NotifierIcon.Question:
                    imageIcon.Source = IconLoader.Question;
                    break;
                case NotifierIcon.Error:
                    imageIcon.Source = IconLoader.Error;
                    break;
                case NotifierIcon.Delete:
                    imageIcon.Source = IconLoader.Delete;
                    break;
                case NotifierIcon.Summary:
                    imageIcon.Source = IconLoader.Summary;
                    break;
                default:
                    imageIcon.Source = null;
                    break;
            }
        }

	    public NotifierView(string title, string message, NotifierIcon icon, NotifierButtons buttons)
            : this(title, message, icon)
        {
            defineButtonsAndCommands(buttons);
            setDefaultAndCancelButton(NotifierDefaultButton.FirstButton);
        }

        public NotifierView(string title, string message, NotifierIcon icon, string[] buttons)
            : this(title, message, icon)
        {
            NotifierButtons buttonDefiniton = NotifierButtons.CustomLeft;
            if (buttons.Length == 2)
                buttonDefiniton = NotifierButtons.CustomLeftMiddle;
            else if (buttons.Length == 3)
                buttonDefiniton = NotifierButtons.CustomLeftMiddleRight;

            defineButtonsAndCommands(buttonDefiniton, buttons);
            setDefaultAndCancelButton(NotifierDefaultButton.FirstButton);
        }

        public NotifierView(string title, string message, NotifierIcon icon, NotifierButtons buttons, NotifierDefaultButton defaultButton)
            : this(title, message, icon, buttons)
        {
            setDefaultAndCancelButton(defaultButton);
        }

        public NotifierView(string title, string message, NotifierIcon icon, string[] buttons, NotifierDefaultButton defaultButton)
            : this(title, message, icon, buttons)
        {
            setDefaultAndCancelButton(defaultButton);
        }        
        #endregion

        #region Event Handlers
        private void loaded(object sender, RoutedEventArgs e)
        {
            DialogResult = null;
        }

        private void ok(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Ok;
            DialogResult = true;
        }

        private void yes(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Yes;
            DialogResult = true;
        }

        private void no(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.No;
            DialogResult = false;
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Cancel;
            DialogResult = false;
        }

        private void retry(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Retry;
            DialogResult = true;
        }

        private void left(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Custom;
            customButtonClicked = Convert.ToInt32((buttonLeft.Command as RoutedCommand)?.Name);
            DialogResult = true;
        }

        private void middle(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Custom;
            customButtonClicked = Convert.ToInt32((buttonMiddle.Command as RoutedCommand)?.Name);
            DialogResult = true;
        }

        private void right(object sender, RoutedEventArgs e)
        {
            buttonClicked = NotifierResult.Custom;
            customButtonClicked = Convert.ToInt32((buttonRight.Command as RoutedCommand)?.Name);
            DialogResult = true;
        }

        #endregion

        #region Methods
        private void defaultInitialization()
        {
            textboxMessage.TextWrapping = TextWrapping.Wrap;
            imageIcon.Source = IconLoader.Error;

            defineButtonsAndCommands(NotifierButtons.Ok);
            setDefaultAndCancelButton(NotifierDefaultButton.FirstButton);
        }

        private void defineButtonsAndCommands(NotifierButtons buttonsToShow, string[] buttons = null)
        {
            notifierButtons = buttonsToShow;

            clearCommandBidings();
            if (buttonsToShow == NotifierButtons.Ok)
                setOkButton();
            else if (buttonsToShow == NotifierButtons.OkCancel)
                setOkCancelButtons();
            else if (buttonsToShow == NotifierButtons.RetryCancel)
                setRetryCancelButtons();
            else if (buttonsToShow == NotifierButtons.YesNo)
                setYeNoButtons();
            else if (buttonsToShow == NotifierButtons.YesNoCancel)
                setYesNoCancelButtons();
            else if (buttonsToShow == NotifierButtons.CustomLeft || buttonsToShow == NotifierButtons.CustomLeftMiddle || buttonsToShow == NotifierButtons.CustomLeftMiddleRight)
                setCustomButtons(buttons);
        }

        private void clearCommandBidings()
        {
            buttonLeft.Command = null;
            buttonMiddle.Command = null;
            buttonRight.Command = null;
            CommandBindings.Clear();
        }

        private void setDefaultAndCancelButton(NotifierDefaultButton defaultButton)
        {
            buttonLeft.IsDefault = false;
            buttonLeft.IsCancel = false;

            buttonMiddle.IsDefault = false;
            buttonMiddle.IsCancel = false;

            buttonRight.IsDefault = false;
            buttonRight.IsCancel = false;

            buttonRight.IsCancel = true;
            switch (notifierButtons)
            {
                case NotifierButtons.Ok:
                    buttonRight.IsDefault = true;
                    break;
                case NotifierButtons.CustomLeft:
                    buttonLeft.IsDefault = true;
                    break;
                case NotifierButtons.OkCancel:
                case NotifierButtons.RetryCancel:
                case NotifierButtons.YesNo:
                    if (defaultButton == NotifierDefaultButton.FirstButton)
                        buttonMiddle.IsDefault = true;
                    else if (defaultButton == NotifierDefaultButton.SecondButton ||
                             defaultButton == NotifierDefaultButton.ThirdButton)
                        buttonRight.IsDefault = true;
                    break;
                case NotifierButtons.CustomLeftMiddle:
                    if (defaultButton == NotifierDefaultButton.FirstButton)
                        buttonLeft.IsDefault = true;
                    else if (defaultButton == NotifierDefaultButton.SecondButton ||
                             defaultButton == NotifierDefaultButton.ThirdButton)
                        buttonMiddle.IsDefault = true;
                    break;
                case NotifierButtons.YesNoCancel:
                case NotifierButtons.CustomLeftMiddleRight:
                    if (defaultButton == NotifierDefaultButton.FirstButton)
                        buttonLeft.IsDefault = true;
                    else if (defaultButton == NotifierDefaultButton.SecondButton)
                        buttonMiddle.IsDefault = true;
                    else if (defaultButton == NotifierDefaultButton.ThirdButton)
                        buttonRight.IsDefault = true;
                    break;
            }
        }

        private void setOkButton()
        {
            buttonLeft.Visibility = Visibility.Collapsed;
            buttonMiddle.Visibility = Visibility.Collapsed;
            buttonRight.Visibility = Visibility.Visible;

            var text = Properties.Resources.Ok.ToUpper();
            addButtonCommand("OkCommand", text, buttonRight, ok, null);
        }

        private void setOkCancelButtons()
        {
            buttonLeft.Visibility = Visibility.Collapsed;
            buttonMiddle.Visibility = Visibility.Visible;
            buttonRight.Visibility = Visibility.Visible;

            var text = Properties.Resources.Ok.ToUpper();
            addButtonCommand("OkCommand", text, buttonMiddle, ok, null);
            text = Properties.Resources.Cancel.ToUpper();
            addButtonCommand("CancelCommand", text, buttonRight, cancel, null);
        }

        private void setRetryCancelButtons()
        {
            buttonLeft.Visibility = Visibility.Collapsed;
            buttonMiddle.Visibility = Visibility.Visible;
            buttonRight.Visibility = Visibility.Visible;

            var text = Properties.Resources.Retry.ToUpper();
            addButtonCommand("RetryCommand", text, buttonMiddle, retry, null);
            text = Properties.Resources.Cancel.ToUpper();
            addButtonCommand("CancelCommand", text, buttonRight, cancel, null);
        }

        private void setYeNoButtons()
        {
            buttonLeft.Visibility = Visibility.Collapsed;
            buttonMiddle.Visibility = Visibility.Visible;
            buttonRight.Visibility = Visibility.Visible;

            string text = Properties.Resources.Yes.ToUpper();
            addButtonCommand("YesCommand", text, buttonMiddle, yes, null);
            text = Properties.Resources.No.ToUpper();
            addButtonCommand("NoCommand", text, buttonRight, no, null);
        }

        private void setYesNoCancelButtons()
        {
            buttonLeft.Visibility = Visibility.Visible;
            buttonMiddle.Visibility = Visibility.Visible;
            buttonRight.Visibility = Visibility.Visible;

            string text = Properties.Resources.Yes.ToUpper();
            addButtonCommand("YesCommand", text, buttonLeft, yes, null);
            text = Properties.Resources.No.ToUpper();
            addButtonCommand("NoCommand", text, buttonMiddle, no, null);
            text = Properties.Resources.Cancel.ToUpper();
            addButtonCommand("CancelCommand", text, buttonRight, cancel, null);
        }

        private void setCustomButtons(string[] buttons)
        {
            if (buttons == null || buttons.Length == 0) return;

            buttonLeft.Visibility = Visibility.Visible;
            buttonMiddle.Visibility = Visibility.Collapsed;
            buttonRight.Visibility = Visibility.Collapsed;

            addButtonCommand("0", buttons[0], buttonLeft, left, null);
            if (buttons.Length == 1) return;

            buttonMiddle.Visibility = Visibility.Visible;
            addButtonCommand("1", buttons[1], buttonMiddle, middle, null);
            if (buttons.Length == 2) return;

            buttonRight.Visibility = Visibility.Visible;
            addButtonCommand("2", buttons[2], buttonRight, right, null);
        }

	    private void addButtonCommand(string name, string text, Button buttonAffected, ExecutedRoutedEventHandler eventHandler, KeyGesture hotKey)
        {
            var command = new RoutedUICommand(text, name, typeof(NotifierView));
            if (hotKey != null)
                command.InputGestures.Add(hotKey);

            buttonAffected.Command = command;
            var binding = new CommandBinding(command, eventHandler);
            CommandBindings.Add(binding);

            buttonAffected.Content = text;
        }
        #endregion

        #region Overriding Methods
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
        #endregion
    }
}