#nullable enable
namespace Core.FTUE.Presenters
{
    public sealed class TextPresenter : IFTUEPresenter
    {
        private readonly IFTUETextView textView;
        private readonly string message;

        public TextPresenter(IFTUETextView textView, string message)
        {
            this.textView = textView;
            this.message = message;
        }

        public void Show() => this.textView.ShowText(this.message);
        public void Hide() => this.textView.HideText();
    }
}
