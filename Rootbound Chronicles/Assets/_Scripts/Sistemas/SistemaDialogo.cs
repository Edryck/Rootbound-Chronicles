using Unity;
using TMPro;
using System.Collections;
using UnityEngine;

public class SistemaDialogo : MonoBehaviour
{
    public static SistemaDialogo instance; // Para facilitar o acesso

    [Header("Configuração da UI")]
    public GameObject caixaDialogo;
    public TextMeshProUGUI textoUI;

    [Header("Configuração do Efeito")]
    // Velocidade de Digitação, quanto menor, mais rápido
    public float velocidadeDigitacao = 0.05f;

    private Coroutine digitandoCoroutine;
    private bool estaAberto = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Para sempre começar com ele fechado
        FecharDialogo();
    }

    void Update()
    {
        // Fecha se apertar E ou Espaço
        if (estaAberto && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
        {
            // TODO: Se estiver digitando ainda, ele completa o texto de forma instantanea
            FecharDialogo();
        }
    }

    public void MostrarTexto(string textoParaEscrever)
    {
        // Se estiver mostrando algo, para e começa de novo
        if (digitandoCoroutine != null) StopCoroutine(digitandoCoroutine);

        caixaDialogo.SetActive(true);
        estaAberto = true;

        // Inicia o efeito de digitação
        digitandoCoroutine = StartCoroutine(EfeitoDigitacao(textoParaEscrever));
    }

    IEnumerator EfeitoDigitacao(string texto)
    {
        textoUI.text = ""; // Para limpar o texto anterior

        foreach (char letra in texto.ToCharArray())
        {
            textoUI.text += letra; // Adiciona uma letra
            yield return new WaitForSeconds(velocidadeDigitacao); // Espera um pouco
        }
    }

    public void FecharDialogo()
    {
        caixaDialogo.SetActive(false);
        estaAberto = false;
        textoUI.text = "";
    }
}