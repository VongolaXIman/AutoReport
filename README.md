
# 專案
1. 請將「ApiTemplate」全部取代為所需的專案名(注意.sln內要改)
2. 將專案支線push上去後，才可以去SaasTestServer、SassQAServer、SassUATServer專案幫已推上去的支線進行gateway、jenkinsfile、yml的設定，不然gateway吃不到對應的服務會死掉




# 檔案

## NuGet.Config
## Jenkinsfile
### 需修改參數
`image_name`參數修改為該專案的 image name(image name的命名要跟放入SassQAServer專案內的image name設定一致，建議**全小寫**)

### base docker image
目前 test 時使用的 image 是 core 3.1，若使用別的版本請自行修改 image
## Dockerfile
### 產生步驟
1. 在 visual studio 對API專案(需先設定完各專案間的參考)按右鍵「加入 Docker 支援」(目標OS為Linux)產生`Dockerfile`和`.dockerignore`檔

2. 將`Dockerfile`移到專案根目錄的build資料夾中(和`NuGet.Config`、`.gitignore`一起)，用檔案總管移動好後再透過VS重新加入

3. 修改`Dockerfile`
   ```Dockerfile   
   # 加入 csproj 檔
   COPY ["XXX/XXX.csproj", "XXX/"] 
   # 將 nuget.config 檔複製到 container 內
   COPY ["NuGet.Config", "NuGet.Config"] 
   # 在 dotnet restore 的時候指定 nuget.config
   RUN dotnet restore "YYY/YYY.csproj" --configfile "NuGet.Config"
   
   # ~~~中間省略~~~
   
   # dotnet build 時不執行 restore
   RUN dotnet build "YYY.csproj" -c Release -o /app/build --no-restore

   # ~~~中間省略~~~
   
   # dotnet publish 時不執行 restore
   RUN dotnet publish "YYY.csproj" -c Release -o /app/build --no-restore
   ```
# CI/CD設定

   ## GitLab

   至「設定 > 整合」幫各支線新增Webhook

   1. `URL`填「{jenkins位置}/gitlab-webhook/post」，目前為「http://192.168.190.1:8080/gitlab-webhook/post」
   
   2. `Trigger`中的`Push events`勾起並填支線名稱，如「develop」
   
   3. `Trigger`中的`Tag push events`、`Merge request events`鉤起

   ## Jenkins

新增作業

1. `item name`寫專案的名稱，最下方選`Multibranch Pipeline`，下一步。
2. 填好`Display Name`、`Description`取消勾選`Disable`
3. Branch Sources
   * `Add source`選GitLab Projet
   * `Checkout Credentials	`選Jenkins開頭的
   * `Owner`填專案在GoitLab中的群組名稱，填完後`Projects`選擇對應專案
   * 依觸發條件填好`Behaviours`，目前常用設定如下:
     * `Discover branches`: All branchees
     * `Discover tags`
     * `Clean before checkout`: 勾起`Delete untracked nested repositories`
     * `Override GitLab hook management modes`
       * `Web Hook Strategy`: `Use System credentials for Web Hook management`
       * `System Hook Strategy`: `Disable System Hook management`
   * `Build strategies`依想要建置的支線新增`Named Branches`，或是依tag新增`tags`
4. Orphaned Item Strategy
   * `Days to keep old items`填1，`Max # of old items to keep`填10

## Sass___Server專案

不同環境對應不同的專案:SaasTestServer、SassQAServer、SassUATServer

要新增yml檔並修改Jenkinsfile、gateway.conf，詳情請見SassQAServer的README

http://192.168.90.40/bpodev/server/SaasQAServer