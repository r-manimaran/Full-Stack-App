def call(String imageName, String path){
    sh """
        cd ${path}
        docker build -t ${imageName}:latest .
    """
}