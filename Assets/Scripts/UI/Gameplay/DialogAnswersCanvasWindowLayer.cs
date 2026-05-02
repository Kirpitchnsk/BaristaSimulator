namespace Arenar.Services.UI {
    public class DialogAnswersCanvasWindowLayer : CanvasWindowLayer {
        public DialogAnswerUiVisual[] DialogAnswers;

        public void SetLayerEnabled(bool isEnabled) {
            gameObject.SetActive(isEnabled);
        }
    }
}