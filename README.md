# Fox Wallet

O **Fox Wallet** é um sistema de controle financeiro pessoal e para pequenos negócios, desenvolvido em **C# com .NET e WPF**, seguindo o padrão de arquitetura **MVVM**.

O objetivo do projeto é facilitar o controle de receitas e despesas de forma simples, organizada e sem a complexidade de planilhas.

---

## Funcionalidades

- Controle de receitas e despesas
- Relatórios mensais e anuais
- Exportação de relatórios em PDF e CSV
- Interface simples e intuitiva

---

## Tecnologias Utilizadas

- C#
- .NET
- WPF (Windows Presentation Foundation)
- MVVM (Model-View-ViewModel)
- System.Text.Json
- PDFSharp-GDI

---

## Arquitetura

O projeto utiliza o padrão **MVVM**, separando responsabilidades:

- **View**: Interface do usuário (XAML)
- **ViewModel**: Comunicação entre View e lógica
- **Services (Model)**: Regras de negócio, persistência e relatórios

---

## Estrutura do Projeto

- `Commands` → Comandos reutilizáveis
- `Entities` → Entidades e enumerações
- `Resources` → Estilos e recursos visuais
- `Services` → Regras de negócio e persistência
- `ViewModels` → Lógica entre Views e Services
- `Views` → Interfaces da aplicação

---

## Próximos Passos

- Módulo de investimentos
- Relatórios mais avançados
- Sistema de configurações
- Lançamentos recorrentes

---

## Licença

Este projeto está sob a licença **MIT**.  
Sinta-se à vontade para usar, modificar e contribuir.
