namespace TurneroApi.Services.Session;

public static class Hasher
{
    public static string Cod(string pass)
    {
        bool wEsimPar = true;
        int n = 1;
        int i = pass.Length;
        string cPass = "";

        while (i != 0)
        {
            char sCaracter = pass[i - 1];

            if (wEsimPar)
            {
                sCaracter = (char)(sCaracter + n);
                if (sCaracter == '\'' || sCaracter == '|')
                {
                    sCaracter = (char)(sCaracter + 2);
                }
                wEsimPar = false;
            }
            else
            {
                sCaracter = (char)(sCaracter - n);
                if (sCaracter == '\'' || sCaracter == '|')
                {
                    sCaracter = (char)(sCaracter + 2);
                }
                wEsimPar = true;
            }

            cPass += sCaracter;
            i--;
            n++;
        }

        return cPass;
    }

    public static string Decod(string pass)
    {
        int n = pass.Length;
        bool wEsimPar = (n % 2 == 0);
        int i = pass.Length;
        string dPass = "";

        while (i != 0)
        {
            char sCaracter = pass[i - 1];

            if (wEsimPar)
            {
                sCaracter = (char)(sCaracter + n);
                if (sCaracter == '\'' || sCaracter == '|')
                {
                    sCaracter = (char)(sCaracter - n);
                }
                wEsimPar = false;
            }
            else
            {
                sCaracter = (char)(sCaracter - n);
                if (sCaracter == '\'' || sCaracter == '|')
                {
                    sCaracter = (char)(sCaracter - n);
                }
                wEsimPar = true;
            }

            dPass += sCaracter;
            i--;
            n--;
        }

        return dPass;
    }
}
