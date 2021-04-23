push_tag = ''
image_name = 'apitemplate'

pipeline {
    agent any
    environment { 
       HOME = '/tmp'
       DOTNET_CLI_HOME = '/tmp/DOTNET_CLI_HOME'
       DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
       DOTNET_SKIP_FIRST_TIME_EXPERIENCE  = 'true'
       NUGET_XMLDOC_MODE = 'skip'

       DOCKER_IMAGE_NAME = "${image_name}"
    } 
    stages {
        stage('Restore - Build - Test') {
            agent { docker 'mcr.microsoft.com/dotnet/core/sdk:3.1-buster' }
            stages {
                stage('Restore') {
                    steps {
                        sh 'dotnet restore --configfile NuGet.Config'
                    }
                }
                stage('Build') {
                    steps {
                        sh 'dotnet build'
                    }
                }
                stage('Test') {
                    steps {
                        sh 'dotnet test -c Release --logger "trx" --results-directory TestResults --collect:"XPlat Code Coverage" < /dev/null'
                        step([$class: 'MSTestPublisher', testResultsFile: 'TestResults/*.trx', failOnError: true, keepLongStdio: true])
                        step([$class: 'CoberturaPublisher', coberturaReportFile: 'TestResults/**/coverage.cobertura.xml'])
                    }
                }
            }
        }
        stage('Build Image') {
            steps {
                script {
                    docker.build("${DOCKER_IMAGE_NAME}:${env.EXECUTOR_NUMBER}-${env.BUILD_ID}")
                }
            }
        }
        stage('Parallel Set Push Tag') {
            failFast true
            parallel {
                stage('Set Tag - Alpha ') {
                    when {
                        branch 'develop'
                    }
                    steps{
                        script {
                            push_tag = 'alpha'
                        }
                    }
                }
                stage('Set Tag - Beta') {
                    when {
                        branch 'release'
                    }
                    steps{
                        script {
                            push_tag = 'beta'
                        }
                    }
                }
                stage('Set Tag - Release candiate') {
                    when {
                        branch 'staging'
                    }
                    steps{
                        script {
                            push_tag = 'rc'
                        }
                    }
                }
                stage('Set Tag - Production') {
                    when {
                        branch 'master'
                    }
                    steps{
                        script {
                            push_tag = 'latest'
                        }
                    }
                }
                stage('Set Tag - Git Tag') {
                    when {
                        buildingTag()
                    }
                    steps{
                        script {
                            push_tag = TAG_NAME
                        }
                    }
                }
            }
		}
	    // Jenkins 需設置全域變數 PRIVATE_DOCKER_REGISTRY
        stage('Push Image')
        {
            when {
                expression { return push_tag }
                not {
                    environment name: 'PRIVATE_DOCKER_REGISTRY', value: ''
                }
            }
            steps {
                script {
                    docker.withRegistry("${env.PRIVATE_DOCKER_REGISTRY}") {
                        docker.image("${DOCKER_IMAGE_NAME}:${env.EXECUTOR_NUMBER}-${env.BUILD_ID}").push(push_tag)
                    }
                }
            }
        }
        stage('Clean up')
        {		
            steps {
                script {
                    currentBuild.result = 'SUCCESS'
                }
            }
        }
    }    
    post {
        always{
            cleanWs()
        }
        success {
            slackNotify('Success')
        }
        failure {
            slackNotify('Failure')
        }
    }
}

def slackNotify(buildStatus) {
    // Default values
    def colorCode = '#DC3545'

    switch(buildStatus) {
        case 'Success':
            colorCode = '#28A745'
            break;
        case 'Failure':
            colorCode = '#DC3545'
            break;
    }

    def subject = "${env.JOB_NAME} - ${env.BUILD_DISPLAY_NAME} ${buildStatus} after ${currentBuild.durationString} (<${env.BUILD_URL}|Open>)"
    def summary = subject
    if(push_tag?.trim()) {
        def image_info = "build and push ${image_name}:${push_tag} to ${env.PRIVATE_DOCKER_REGISTRY}"
        def tag_list_link = "(<${env.PRIVATE_DOCKER_REGISTRY}/v2/${image_name}/tags/list|Open>)"
        
        summary += "\n"
        summary += "${image_info} ${tag_list_link}"
    }

    // Send notifications
    slackSend (color: colorCode, message: summary)
}