# nfi-encomendas
> breve resumo para arrancar a aplicação

NFI Encomendas:
## Stack Tecnologica:
    - Backend
        - ASP.NET 4.5
        - MicrosoftSQL Server
    - Frontend (cliente)
        - Angular

---


## 1 - Base de dados; Restaurar em servidor local
O dump está no ficheiro db_encomendas_dump_XXXXX no repositório e pode ser restaurado a partir de lá.

A base de dados deverá ser criada primeiro e no dump ignorar as linhas de criação da base de dados e depois renomear o nome para a base de dados criada anteriormente

No exemplo que se segue a base de dados terá o nome de **“nfi_encomendas”**, 

## 2 - Configurar connecção no ficheiro web.config
Neste exemplo, a connection string deverá ter os dados de acesso ao servidor, dentro do node *** < connectionStrings> ***

```
<...>
<connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=.\SQLEXPRESS;initial catalog=nfi_encomendas;persist security info=True;user id=sa;password=Dev1234!;"  providerName="System.Data.SqlClient" />

    <add name="AuthContext" connectionString="Data Source=.\SQLEXPRESS;initial catalog=nfi_encomendas;persist security info=True;user id=sa;password=Dev1234!"  providerName="System.Data.SqlClient" />
</connectionStrings>
</...>
```


> NOTA: configurar com IIS Local foi a forma mais perto do ambiente de produção encontrada. Pode ser usado o IIS Express mudando apenas as rotas da API no frontend.

## 3 - Configurar o projeto para uso do IIS local (config recomendada)


Abrir a solution no Visual Studio e garantir que os NuGet Packages estão "restaured".

Uma vez que o projeto está separado em 2 partes, API e frontend, de modo a facilitar o desenvolvimento o mais parecido como ambiente em produção, recomendo a usar o IIS local para arrancar o projeto.

***Deverá ter o IIS instalado no Windows***
No projeto "NfiEncomendas.WebServer", ir às “Properties”, “Web” (Lado direito), Secção de "Server":

    - Alterar o dropdown para "Local IIS"
    - Colocar o link como "http://localhost/nfi_encomendas/apiserver_app/" e clicar em “Create Virtual Directory”    
    Selecionar a opção “Override application root URL”

## 4 - Configurar IIS
Depois de configurado no Visual Studio, e arranque da API, deverá aparecer no IIS Manager a o website “nfi_encomendas”. 

A raiz deste site deve ser alterado para a pasta onde está o projeto “WebClient”.

No IIS manager, na pasta “nfi_encomendas”, clicar no botão direito e ir “Manage Virtual Directory > “Advanced Settings” e mudar a “Physical Path”.

##  5 - *** Alterar as rotas em front-end (caso necessário) ***

Na pasta do font-end, editar o ficheiro “js/app.js” e editar a variável “serviceBase” para o caminho exato, ou relativo, como for preferivel.
Ter em atenção a esta alteração quando se enviar para produção.

