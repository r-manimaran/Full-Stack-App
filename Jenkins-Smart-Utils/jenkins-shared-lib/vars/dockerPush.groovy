def call(String imageName){
    sh """
        docker push ${imageName}:latest
    """
}