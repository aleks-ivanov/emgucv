# --------------------------------------------------------
#  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND HAVE_WINDESKTOP_ARM64)

  PROJECT(Emgu.runtime.windows.msvc.rt.arm64.nuget)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 

  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGU_WINDOWS_MSVCRT_ARM64_NUGET_ID}.${EMGU_WINDOWS_MSVCRT_NUGET_VERSION}.nupkg")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

ENDIF()
