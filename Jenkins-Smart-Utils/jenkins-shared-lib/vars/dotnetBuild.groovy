
def call(String path){
    sh """
        cd ${path}
        dotnet restore
        dotnet build --configuration Release
        dotnet publish --configuration Release --output published
    """
}