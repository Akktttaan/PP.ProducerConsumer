using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using App;

namespace Client;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProducerConsumer<char> _producerConsumer = new (16);

        public MainWindow()
        {
            InitializeComponent();
            CreateConsumer(x => char.IsDigit(x), Digits).Start();
            CreateConsumer(x => char.IsLetter(x), Symbols).Start();
            CreateConsumer(x => !char.IsLetterOrDigit(x), Others).Start();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _producerConsumer.Produce(InputBox.Text[^1]);
        }

        private Thread CreateConsumer (Predicate<char> predicate, TextBox textBox)
        {
            return new Thread(() =>
            {
                while (true)
                {
                    var element = _producerConsumer.Consume(predicate);
                    Dispatcher.Invoke(() =>
                    {
                        textBox.Text += element;
                    });
                }
            });
        }
    }
