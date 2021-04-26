## How do i use this

App uses swagger to see implemented endpoints and their documentation: http://localhost:57678/swagger/index.html

~~Before using the api you need to generate database.
To do that go to package manager console, select FlowerShop.Infrastructure as default project and use command:
PM> update-database~~

## Azure Database 💥 
App uses azure database connection. Full connection string can be found in Google drive. If you need to access the azure portal with database resources message **Augustas**.
***Please don't create any more additional databases in the server as it's not free. If you accidently created a new server message **Augustas**.***

# Code style: 📃
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions

# Branch rules: 🌿

👉There are one main branch - **master**.  
👉**master** contains only changes that are stable and are reviewed by a teammate
👉Before developing a new **feature** review your coleague Pr's.  
👉If you want to start developing a new **feature**, then create a new branch from **master**. Branch name should reflect jira tasks which it accomplishes for example: **S5-19-20**. After you finish developing it, create a PR to master branch. After successful merge, do not forget to **delete** the feature branch! (It can be done automatically on GitHub) 
👉If you want to fix a **bug**, then also create a branch from master and naming should be: **S5bug-19**  

# PR and Commit Rules: 👇 
Commit as frequently as possible. Avoid huge commits, with many code changes. Commit summary or description should include what was changed/added in the code. Pr naming should reflect jira tasks
PR can be merged only after **atleast one teammate** approved it.
After creating a PR review it **yourself** in order to avoid any mistakes or leftover code.
