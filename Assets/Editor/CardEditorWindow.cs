using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CardEditorWindow : EditorWindow
{
    [MenuItem("Tools/Card Editor Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<CardEditorWindow>();
        window.titleContent = new GUIContent("Card Editor");
        window.minSize = new Vector2(500, 350);
    }

    private void OnEnable()
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CardEditorWindow.uxml");
        TemplateContainer treeAsset = original.CloneTree();
        rootVisualElement.Add(treeAsset);

        CreateCardListView();

        AddButtonFunctionality();
    }

    private void AddButtonFunctionality()
    {
        var button = rootVisualElement.Query<Button>("add-new-card").First();
        button.clicked += () =>
        {
            CardData asset = ScriptableObject.CreateInstance<CardData>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/CardData/Card.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            CreateCardListView();
        };
    }

    private void CreateCardListView()
    {
        FindAllCards(out CardData[] cards);

        ListView cardList = rootVisualElement.Query<ListView>("card-list-view").First();
        cardList.makeItem = () => new Label();
        cardList.bindItem = (element, i) => (element as Label).text = cards[i].name;

        cardList.itemsSource = cards;
        cardList.itemHeight = 16;
        cardList.selectionType = SelectionType.Single;

        cardList.onSelectionChange += (enumerable) =>
        {
            foreach (Object it in enumerable)
            {
                VisualElement cardInfoBox = rootVisualElement.Query<VisualElement>("cad-info-box");
                cardInfoBox.Clear();

                CardData card = it as CardData;

                SerializedObject serializedCard = new SerializedObject(card);
                SerializedProperty cardProperty = serializedCard.GetIterator();
                cardProperty.Next(true);

                while (cardProperty.NextVisible(false)) //go into children?
                {
                    PropertyField prop = new PropertyField(cardProperty);

                    //we disabled the m_script property so we don't allow anyone to change the scripting reference
                    prop.SetEnabled(cardProperty.name != "m_Script");
                    prop.Bind(serializedCard);
                    cardInfoBox.Add(prop);

                    if (cardProperty.name.Equals("name", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        prop.RegisterValueChangeCallback((ev) =>
                        {
                            CreateCardListView();
                        });
                    }
                    //check for image ?
                }
            }
        };

        cardList.Refresh();
    }

    private void FindAllCards(out CardData[] cards)
    {
        var guids = AssetDatabase.FindAssets("t:CardData");

        cards = new CardData[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            cards[i] = AssetDatabase.LoadAssetAtPath<CardData>(path);
        }

        cards = cards.OrderBy(c => c.CardName).ToArray();
    }
}
