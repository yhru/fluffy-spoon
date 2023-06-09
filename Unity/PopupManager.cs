using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class PopupManager : MonoBehaviour
{
    public GameObject popup;
    //public ToastManager toastManager;

    private Button popupButton;
    private Button socialButton;
    private string baseUrl = "https://naturally-form-guard-bright.trycloudflare.com";

    private void Start()
    {
        socialButton = GameObject.Find("SocialButton").GetComponent<Button>();
        popupButton = GameObject.Find("PopupButton").GetComponent<Button>();

        ChangeInvisible(socialButton);
        popup.SetActive(false);
    }

    public void OpenClosePopup()
    {
        popup.SetActive(!popup.activeSelf);
    }

    public void OpenPopup()
    {
        popup.SetActive(true);
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }

    public async void SubmitForm()
    {
        TMP_InputField firstNameInput = popup.transform.Find("FormPanel/FirstNameInput").GetComponent<TMP_InputField>();
        TMP_InputField emailInput = popup.transform.Find("FormPanel/EmailAddressInput").GetComponent<TMP_InputField>();

        string firstName = firstNameInput.text;
        string emailAddress = emailInput.text;

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(emailAddress))
        {
            await CreateProspectDolibarr(firstName, emailAddress);
            ClosePopup();
            ChangeVisible(socialButton);
            ChangeInvisible(popupButton);
        }
        else
        {
            Debug.Log("Veuillez renseigner votre prénom et votre email");
            //toastManager.ShowToast("Message à afficher", Color.red);
        }
    }

    private async Task CreateProspectDolibarr(string firstName, string emailAddress)
    {
        string apiUrl = $"{baseUrl}/api/index.php/thirdparties/";

        Dictionary<string, string> postData = new Dictionary<string, string>();
        postData["name"] = firstName;
        postData["email"] = emailAddress;
        postData["client"] = "2";
        postData["code_client"] = "auto";

        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("DOLAPIKEY", "GryqNRT7J5t41Z86wg1rGL62CJnx6ruE");

            var response = await httpClient.PostAsync(apiUrl, new FormUrlEncodedContent(postData));

            if(response.IsSuccessStatusCode)
            {
                Debug.Log("Utilisateur enregistré avec succès");
                //toastManager.ShowToast("Utilisateur enregistré avec succès", Color.green);
            }
            else
            {
                Debug.Log(response.ReasonPhrase);
                //toastManager.ShowToast(response.ReasonPhrase, Color.red);
            }
        }
    }

    private void SetButtonInteractableWithAlpha(Button button, float alpha, bool interactable)
    {
        button.interactable = interactable;
        Image imageBouton = button.GetComponent<Image>();
        Color couleurActuelle = imageBouton.color;
        couleurActuelle.a = alpha;
        imageBouton.color = couleurActuelle;
    }

    private void ChangeVisible(Button bouton)
    {
        SetButtonInteractableWithAlpha(bouton, 1f, true);
    }

    private void ChangeInvisible(Button bouton)
    {
        SetButtonInteractableWithAlpha(bouton, 0f, false);
    }
}
