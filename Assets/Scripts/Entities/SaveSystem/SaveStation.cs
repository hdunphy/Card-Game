using Assets.Scripts.Controller.SaveSystem;
using Assets.Scripts.GameScene.Entities;
using Assets.Scripts.References;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.SaveSystem
{
    public class SaveStation : MonoBehaviour, IPlayerInteractable
    {
        [SerializeField] private SpriteRenderer SpriteRenderer;
        [SerializeField] private Sprite OffSprite;
        [SerializeField] private Sprite OnSprite;
        [SerializeField] private TMPro.TMP_Text SaveStationText;

        private const string OnEnterText = "Press E to Save";
        private const string OnSaveText = "Saving...";
        private const string OnSavedText = "Saved";

        private void Start()
        {
            SaveStationText.enabled = false;
        }

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

            SaveData.Current.PlayerSceneName = gameObject.scene.name;
            SaveData.Current.Random = Rules.Instance.GetRandom();
            controller.SavePlayerData();
            
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
