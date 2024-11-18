using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections;

namespace Sata.Api.Estoque.Infraestrutura.Util
{
    public class CriadorSQL
    {
        private readonly string tabela;
        private string selectHeader = "";
        private string selectFooter = "";
        private string innerJoin = "";
        private StringBuilder csql;
        private readonly IList campos = new ArrayList();
        private readonly IList valores = new ArrayList();
        private readonly IList where = new ArrayList();
        private readonly IList key = new ArrayList();

        public CriadorSQL(string tabela)
        {
            this.tabela = tabela;
        }

        public void AddCampo(string nome, object valor)
        {
            campos.Add(nome);

            if (valor == null)
                valores.Add("null");
            else
            {
                if (valor is string texto)
                    valores.Add(string.Format("'{0}'", texto));

                else if (valor is int inteiro)
                    valores.Add(inteiro);

                else if (valor is decimal vlrDecimal)
                    valores.Add(vlrDecimal.ToString().Replace(",", "."));

                else if (valor is DateTime data)
                {
                    if (data.ToShortDateString() != "01/01/0001")
                        valores.Add(string.Format("TO_DATE('{0}', 'dd/mm/yyyy')", data.ToShortDateString()));
                    else
                        valores.Add("null");
                }

            }
        }

        //TODO - Incluir outras cláusulas
        public void AddWhere(string nome, object valor)
        {
            string clausula = "";

            if (where.Count == 0)
                where.Add(" WHERE 1=1 ");

            clausula = " AND " + nome + " = ";

            if (valor is string texto)
                where.Add(clausula + string.Format("'{0}'", texto));

            else if (valor is int inteiro)
                where.Add(clausula + inteiro);

            else if (valor is decimal vlrDecimal)
                where.Add(clausula + vlrDecimal.ToString().Replace(",", "."));

            else if (valor is DateTime data)
                where.Add(clausula + string.Format("TO_DATE('{0}', 'dd/mm/yyyy')", data));
        }

        public void AddSelectHeader(string clausula)
        {
            selectHeader = clausula;
        }
        public void AddSelectFooter(string clausula)
        {
            selectFooter = clausula;
        }
        public void AddInnerJoin(string clausula)
        {
            innerJoin = clausula;
        }

        private string TranslateValue(string clausula, object valor)
        {
            string retorno = "";

            if (valor == null)
                return "";

            if (valor is string texto)
            {
                if (!string.IsNullOrWhiteSpace(texto))
                    retorno = string.Format(clausula, string.Format("'{0}'", texto));

            }
            else if (valor is int inteiro)
            {
                retorno = string.Format(clausula, string.Format("{0}", inteiro));
            }
            else if (valor is decimal vlrDecimal)
            {
                if (vlrDecimal != 0)
                    retorno = string.Format(clausula, string.Format("{0}", vlrDecimal.ToString().Replace(",", ".")));
            }
            else if (valor is DateTime data)
            {
                if (data != null)
                    retorno = string.Format(clausula, string.Format("TO_DATE('{0}', 'dd/mm/yyyy')", data.ToShortDateString()));
            }
            else if (valor is int?[] arrInteiro && arrInteiro.Length > 0 && !arrInteiro.Any(x => x is null))
            {
                retorno = string.Format(clausula, string.Format("{0}", string.Join(",", arrInteiro)));
            }

            return retorno;
        }

        public void AddWhereValid(string clausula, object valor)
        {
            string retorno = TranslateValue(clausula, valor);
            if (!string.IsNullOrWhiteSpace(retorno))
                where.Add(retorno);
        }

        public void AddWhereValid(string clausula, int[] valor)
        {
            string sep = "";
            string retorno = "";
            if (valor != null)
            {
                foreach (object item in valor)
                {
                    retorno += TranslateValue(sep + "{0}", item);
                    sep = ", ";
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                    where.Add(string.Format(clausula, retorno));
            }
        }

        public void AddWhereValid(string clausula, string[] valor)
        {
            string sep = "";
            string retorno = "";
            if (valor != null)
            {
                foreach (object item in valor)
                {
                    retorno += TranslateValue(sep + "{0}", item);
                    sep = ", ";
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                    where.Add(string.Format(clausula, retorno));
            }
        }

        public void AddWhereCustom(string clausula)
        {
            if (where.Count == 0)
                where.Add(" WHERE 1=1 ");

            where.Add(clausula);
        }

        public void AddWhere(Dictionary<string, object> CampoValor)
        {
            foreach (var item in CampoValor)
                AddWhere(item.Key.ToString(), item.Value);
        }

        public void AddCampo(Dictionary<string, object> CampoValor)
        {
            foreach (var item in CampoValor)
                AddCampo(item.Key.ToString(), item.Value);

        }

        public void AddCampo(string nome)
        {
            AddCampo(nome, string.Empty);
        }

        public void AddCampo(List<string> nome)
        {
            foreach (var item in nome)
                AddCampo(item, string.Empty);
        }


        public void AddKey(string nome)
        {
            if (key.Count == 0)
            {
                key.Add(nome);
                return;
            }
            else
                key.Add(key[^1] + ", " + nome);
        }

        public string Insert()
        {
            string strSep = "";
            string strOutput = "";

            csql = new StringBuilder();
            csql.AppendFormat("INSERT INTO {0} (", tabela);
            csql.AppendLine();

            //Loop para adicionar todos os campos do ArrayList de Campos
            for (int i = 0; i < campos.Count; i++)
            {
                csql.Append(strSep + campos[i]);
                strSep = ", ";
            }
            csql.Append(") ");
            csql.AppendLine();

            csql.Append("VALUES ");
            csql.AppendLine();
            csql.Append("(");

            strSep = "";

            //Loop para adicionar todos os valores do ArrayList de Valores
            for (int i = 0; i < valores.Count; i++)
            {
                csql.Append(strSep + valores[i]);
                strSep = ", ";
            }

            csql.Append(") ");
            csql.AppendLine();
            csql.Append(strOutput);

            return csql.ToString();
        }

        public string Update()
        {
            string strSep = "";
            csql = new StringBuilder();
            csql.AppendFormat("UPDATE {0} SET ", tabela);
            csql.AppendLine();

            //Loop para adicionar todos os campos e valores definidos
            for (int i = 0; i < campos.Count; i++)
            {
                csql.AppendFormat("{0} {1} = {2}", strSep, campos[i], valores[i]);
                csql.AppendLine();
                strSep = ", ";
            }

            for (int i = 0; i < where.Count; i++)
            {
                csql.Append(where[i]);
                csql.AppendLine();
            }

            return csql.ToString();
        }

        public string Delete()
        {
            csql = new StringBuilder();
            csql.Append("DELETE FROM ");
            csql.Append(tabela);

            for (int i = 0; i < where.Count; i++)
                csql.Append(where[i]);

            return csql.ToString();
        }

        public string Select()
        {
            string strSep = "";
            csql = new StringBuilder();

            if (!selectHeader.Equals(""))
            {
                csql.AppendLine(selectHeader);
            }
            else
            {
                csql.AppendLine("SELECT ");

                if (campos.Count == 0)
                    csql.Append("*");
                else
                {
                    //Loop para adicionar todos os campos
                    for (int i = 0; i < campos.Count; i++)
                    {
                        csql.AppendLine(strSep + campos[i]);
                        strSep = ", ";
                    }
                }

                csql.AppendLine(" FROM " + tabela);
            }

            if (!string.IsNullOrWhiteSpace(innerJoin))
                csql.AppendLine(innerJoin);

            for (int i = 0; i < where.Count; i++)
                csql.AppendLine(where[i].ToString());

            csql.Append(selectFooter);

            return csql.ToString();
        }
    }
}
