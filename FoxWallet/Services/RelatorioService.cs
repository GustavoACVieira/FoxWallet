using FoxWallet.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FoxWallet.Services {
    public class RelatorioService {
        public readonly ReceitaService receitaService;
        public readonly DespesaService despesaService;

        public RelatorioService(ReceitaService rs, DespesaService ds) {
            receitaService = rs;
            despesaService = ds;
        }

        public void GerarRelatorioMesPDF(MesItem mes, int ano, Action RequestClose) {
            string caminhoMes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"relatorioMensalde{mes.Nome}de{ano}.pdf");

            var rec = receitaService.receitas.Where(r => r.dataReceita.Month == mes.Numero && r.dataReceita.Year == ano);
            var desp = despesaService.despesas.Where(d => d.dataDespesa.Month == mes.Numero && d.dataDespesa.Year == ano);

            var totReceitas = rec.Sum(r => r.Preco);
            var totDespesas = desp.Sum(d => d.Preco);
            var totLiquido = totReceitas - totDespesas;

            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Relatório Financeiro do mês {mes.Nome} de {ano}";

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont Fonttitulo = new XFont("Segoe UI", 18, XFontStyleEx.Bold);
            XFont FontHeader = new XFont("Segoe UI", 12, XFontStyleEx.Bold);
            XFont FontLinha = new XFont("Segoe UI", 11);

            // Título
            gfx.DrawString($"Relatório Financeiro do mês {mes.Nome} de {ano}",
                Fonttitulo,
                XBrushes.Black,
                new XRect(0, 50, page.Width, 40),
                XStringFormats.TopCenter
            );

            // Conteúdo
            double margemEsq = 40;
            double larguraPagina = page.Width - 80;

            double colObs = margemEsq;
            double colTipo = margemEsq + 200;
            double colData = margemEsq + 320;
            double colPreco = margemEsq + 420;

            double y = 120;
            double alturaLinha = 20;

            // Receitas
            gfx.DrawString("RECEITAS", FontHeader, XBrushes.Black, colObs, y);
            y += 25;

            gfx.DrawString("Observação", FontHeader, XBrushes.Black, colObs, y);
            gfx.DrawString("Tipo", FontHeader, XBrushes.Black, colTipo, y);
            gfx.DrawString("Data", FontHeader, XBrushes.Black, colData, y);
            gfx.DrawString("Preço", FontHeader, XBrushes.Black, colPreco, y);

            y += 5;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 15;

            foreach (var r in rec) {
                gfx.DrawString(r.Observacoes ?? "-", FontLinha, XBrushes.Black, colObs, y);
                gfx.DrawString(r.tipoReceita.ToString(), FontLinha, XBrushes.Black, colTipo, y);
                gfx.DrawString(r.dataReceita.ToShortDateString(), FontLinha, XBrushes.Black, colData, y);
                gfx.DrawString(r.Preco.ToString("C"), FontLinha, XBrushes.Black, colPreco, y);

                y += alturaLinha;

                if (y > page.Height - 60) {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 10;
                }
            }

            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            gfx.DrawString($"Total de Receitas: {totReceitas:C}", FontHeader, XBrushes.Black, colPreco - 110, y);

            y += 10;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            // Despesas
            y += 10;
            gfx.DrawString("DESPESAS", FontHeader, XBrushes.Red, colObs, y);
            y += 25;

            gfx.DrawString("Observação", FontHeader, XBrushes.Red, colObs, y);
            gfx.DrawString("Tipo", FontHeader, XBrushes.Red, colTipo, y);
            gfx.DrawString("Data", FontHeader, XBrushes.Red, colData, y);
            gfx.DrawString("Preço", FontHeader, XBrushes.Red, colPreco, y);

            y += 5;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 15;

            foreach (var d in desp) {
                gfx.DrawString(d.Observacoes ?? "-", FontLinha, XBrushes.Red, colObs, y);
                gfx.DrawString(d.tipoDespesa.ToString(), FontLinha, XBrushes.Red, colTipo, y);
                gfx.DrawString(d.dataDespesa.ToShortDateString(), FontLinha, XBrushes.Red, colData, y);
                gfx.DrawString(d.Preco.ToString("C"), FontLinha, XBrushes.Red, colPreco, y);

                y += alturaLinha;

                if (y > page.Height - 60) {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }

            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            gfx.DrawString($"Total de Despesas: {totDespesas:C}", FontHeader, XBrushes.Red, colPreco - 110, y);

            y += 10;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            gfx.DrawString($"TOTAL LÍQUIDO: {totLiquido:C}",
                new XFont("Segoe UI", 14, XFontStyleEx.Bold),
                totLiquido >= 0 ? XBrushes.Blue : XBrushes.DarkRed,
                colPreco - 120,
                y
            );

            document.Save(caminhoMes);
            RequestClose?.Invoke();
        }

        public void GerarRelatorioAnoPDF(ObservableCollection<MesItem> Meses, int ano, Action RequestClose) {
            string caminhoAno = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"relatorioAnualde{ano}.pdf");

            var rec = receitaService.receitas.Where(r => r.dataReceita.Year == ano);
            var desp = despesaService.despesas.Where(d => d.dataDespesa.Year == ano);

            List<decimal> totReceitas = new List<decimal>();
            List<decimal> totDespesas = new List<decimal>();

            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Relatório Financeiro de {ano}";

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont Fonttitulo = new XFont("Segoe UI", 18, XFontStyleEx.Bold);
            XFont FontHeader = new XFont("Segoe UI", 12, XFontStyleEx.Bold);
            XFont FontLinha = new XFont("Segoe UI", 11);

            // Título
            gfx.DrawString($"Relatório Financeiro de {ano}",
                Fonttitulo,
                XBrushes.Black,
                new XRect(0, 50, page.Width, 40),
                XStringFormats.TopCenter
            );

            // Conteúdo
            double margemEsq = 40;
            double larguraPagina = page.Width - 80;

            double colMes = margemEsq;
            double colTotMês = margemEsq + 200;

            double y = 120;
            double alturaLinha = 20;

            // Receitas
            gfx.DrawString("RECEITAS", FontHeader, XBrushes.Black, colMes, y);
            y += 25;

            gfx.DrawString("Mês", FontHeader, XBrushes.Black, colMes, y);
            gfx.DrawString("Total Mês", FontHeader, XBrushes.Black, colTotMês, y);

            y += 5;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 15;

            foreach (var m in Meses) {
                var totRec = rec.Where(r => r.dataReceita.Month == m.Numero).Sum(r => r.Preco);

                totReceitas.Add(totRec);

                gfx.DrawString(m.Nome, FontLinha, XBrushes.Black, colMes, y);
                gfx.DrawString(totRec.ToString("C"), FontLinha, XBrushes.Black, colTotMês, y);

                y += alturaLinha;

                if (y > page.Height - 60) {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 10;
                }
            }

            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            gfx.DrawString($"Total de Receitas: {totReceitas.Sum(r => r):C}", FontHeader, XBrushes.Black, colTotMês - 110, y);

            y += 10;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            // Despesas
            y += 10;
            gfx.DrawString("DESPESAS", FontHeader, XBrushes.Red, colMes, y);
            y += 25;

            gfx.DrawString("Mês", FontHeader, XBrushes.Red, colMes, y);
            gfx.DrawString("Total Mês", FontHeader, XBrushes.Red, colTotMês, y);

            y += 5;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 15;

            foreach (var m in Meses) {
                var totDesp = desp.Where(d => d.dataDespesa.Month == m.Numero).Sum(d => d.Preco);

                totDespesas.Add(totDesp);

                gfx.DrawString(m.Nome, FontLinha, XBrushes.Black, colMes, y);
                gfx.DrawString(totDesp.ToString("C"), FontLinha, XBrushes.Black, colTotMês, y);

                y += alturaLinha;

                if (y > page.Height - 60) {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 10;
                }
            }

            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            gfx.DrawString($"Total de Despesas: {totDespesas.Sum(d => d):C}", FontHeader, XBrushes.Red, colTotMês - 110, y);

            y += 10;
            gfx.DrawLine(XPens.Black, margemEsq, y, page.Width - 40, y);
            y += 20;

            var totLiquido = totReceitas.Sum(r => r) - totDespesas.Sum(d => d);

            gfx.DrawString($"TOTAL LÍQUIDO ANUAL: {totLiquido:C}",
                new XFont("Segoe UI", 14, XFontStyleEx.Bold),
                totLiquido >= 0 ? XBrushes.Blue : XBrushes.DarkRed,
                colTotMês - 120,
                y
            );

            document.Save(caminhoAno);
            RequestClose?.Invoke();
        }

        public void GerarRelatorioMesCSV(MesItem mes, int ano, Action RequestClose) {
            string caminhoMes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"relatorioMensalde{mes.Nome}de{ano}.csv");
            string conteudo = "Total_Receitas;Total_Despesas;Líquido;\n";

            var rec = receitaService.receitas.Where(r => r.dataReceita.Month == mes.Numero && r.dataReceita.Year == ano);
            var desp = despesaService.despesas.Where(d => d.dataDespesa.Month == mes.Numero && d.dataDespesa.Year == ano);

            var totReceitas = rec.Sum(r => r.Preco);
            var totDespesas = desp.Sum(d => d.Preco);

            // Conteúdo
            conteudo += $"{totReceitas};{totDespesas};{totReceitas - totDespesas};\n";
            conteudo += "\nTipo;Data;Descrição;Valor;\n";

            foreach (var Receita in rec)
                conteudo += $"{nameof(Receita)};{Receita.dataReceita.ToShortDateString()};{Receita.Observacoes};{Receita.Preco};\n";

            foreach (var Despesa in desp)
                conteudo += $"{nameof(Despesa)};{Despesa.dataDespesa.ToShortDateString()};{Despesa.Observacoes};{Despesa.Preco};\n";

            File.WriteAllText(caminhoMes, conteudo);
            RequestClose?.Invoke();
        }

        public void GerarRelatorioAnoCSV(ObservableCollection<MesItem> Meses, int ano, Action RequestClose) {
            string caminhoAno = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"relatorioAnualde{ano}.csv");
            string conteudo = "Mês;Receitas;Despesas;Líquido;\n";

            var rec = receitaService.receitas.Where(r => r.dataReceita.Year == ano);
            var desp = despesaService.despesas.Where(d => d.dataDespesa.Year == ano);

            List<decimal> totReceitas = new List<decimal>();
            List<decimal> totDespesas = new List<decimal>();

            // Conteúdo
            foreach (var m in Meses) {
                var totRec = rec.Where(r => r.dataReceita.Month == m.Numero).Sum(r => r.Preco);

                var totDesp = desp.Where(d => d.dataDespesa.Month == m.Numero).Sum(d => d.Preco);

                totDespesas.Add(totDesp);
                totReceitas.Add(totRec);

                conteudo += $"{m.Nome};{totRec};{totDesp};{totRec - totDesp};\n";
            }

            conteudo += "\nTOTAL ANUAL;\n";

            conteudo += $"Receitas;{totReceitas.Sum(r => r)};\n";
            conteudo += $"Despesas;{totDespesas.Sum(d => d)};\n";
            conteudo += $"Total Líquido;{totReceitas.Sum(r => r) - totDespesas.Sum(d => d)};\n";

            File.WriteAllText(caminhoAno, conteudo);
            RequestClose?.Invoke();
        }
    }
}
