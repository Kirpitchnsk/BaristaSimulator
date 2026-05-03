using SibGameJam2026.Settings;

namespace Arenar.Services.UI {
    public class ClientsListCanvasWindowLayer : CanvasWindowLayer {
        public ClientContainerUiVisual[] ClientContainers;

        public void SetClients(GameSettingsData gameData) {
            if (ClientContainers == null)
                return;
            
            for (var i = 0; i < ClientContainers.Length; i++) {
                var container = ClientContainers[i];
                if (container == null)
                    continue;
                
                bool isActive = i < gameData.ClientData.Count;
                container.gameObject.SetActive(isActive);
                if (!isActive)
                    continue;
                var clientData = gameData.ClientData[i];
                container.Initialize(clientData.ClientIcon);
            }
        }

        public void SetSlotCookFailed(int slotIndex) {
            if (ClientContainers == null || slotIndex < 0 || slotIndex >= ClientContainers.Length)
                return;

            var container = ClientContainers[slotIndex];
            if (container == null || !container.gameObject.activeSelf)
                return;

            container.SetClientInteractResult(false);
        }

        public void SetSlotCookSuccess(int slotIndex) {
            if (ClientContainers == null || slotIndex < 0 || slotIndex >= ClientContainers.Length)
                return;

            var container = ClientContainers[slotIndex];
            if (container == null || !container.gameObject.activeSelf)
                return;

            container.SetClientInteractResult(true);
        }
    }
}