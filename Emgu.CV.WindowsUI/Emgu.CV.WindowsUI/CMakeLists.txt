# --------------------------------------------------------
#  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32 AND NOT (NETFX_CORE))
  
    PROJECT(Emgu.CV.WindowsUI)

    SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.WindowsUI.csproj" "" ALL)
      
    #BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/Windows.Desktop/Emgu.CV.Example.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)	

    #IF (EMGU_SIGN_FOUND)
    #  EMGU_SIGN_BINARY(${PROJECT_NAME} ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll )
    #ENDIF()

    INSTALL(
      DIRECTORY 
      ${LIBRARY_OUTPUT_PATH}/Build/Emgu.CV.UI
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)	
    
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Bitmap)
  
ENDIF()
