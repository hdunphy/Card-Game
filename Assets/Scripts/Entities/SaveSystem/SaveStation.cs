using Assets.Scripts.Controller.SaveSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.SaveSystem
{
    public class SaveStationController : MonoBehaviour, IPlayerInteractable
    {
        [SerializeField] private SpriteRenderer SpriteRenderer;
        [SerializeField] private Sprite OffSprite;
        [SerializeField] private Sprite OnSprite;
        [SerializeField] private Text SaveStationText;

        private const string OnEnterText = "Press Up to Save";
        private const string OnSaveText = "Saving...";
        private const string OnSavedText = "Saved";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController controller))
            {
                controller.SetInteraction(this);
                SpriteRenderer.sprite = OnSprite;
                SaveStationText.enabled = true;
                SaveStationText.text = OnEnterText;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController controller))
            {
                controller.SetInteraction(null);
                SpriteRenderer.sprite = OffSprite;
                SaveStationText.enabled = false;
            }
        }

        public void Interact(PlayerController controller)
        {
            SaveStationText.text = OnSaveText;
            SaveData.Current.PlayerPosition = controller.transform.position;
            SaveData.Current.PlayerSceneName = gameObject.scene.name;
            if (SerializationManager.Save(SaveData.Current.SaveName, SaveData.Current))
            {
                Debug.Log("Game Saved");
                StartCoroutine(SaveFlash());
            }
            else
            {
                Debug.LogError("Could not save Save Data");
                SaveStationText.text = "Could not save Save Data";
            }
        }

        private IEnumerator SaveFlash()
        {
            SpriteRenderer.sprite = OffSprite;
            yield return new WaitForSeconds(.25f);
            SpriteRenderer.sprite = OnSprite;
            yield return new WaitForSeconds(.25f);
            SpriteRenderer.sprite = OffSprite;
            yield return new WaitForSeconds(.5f);
            SpriteRenderer.sprite = OnSprite;

            SaveStationText.text = OnSavedText;
        }
    }
}
