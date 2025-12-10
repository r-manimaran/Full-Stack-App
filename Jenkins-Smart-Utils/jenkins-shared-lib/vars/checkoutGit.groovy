def call(String repoUrl, String branch = 'main') {
    // sh """
    //     git clone -b ${branch} ${repoUrl} .
    // """
    git branch: '${branch}',
                    url: '${repoUrl}'
}