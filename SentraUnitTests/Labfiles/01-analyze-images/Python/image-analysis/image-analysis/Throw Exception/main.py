# Unit tests for the given source code
import unittest
from unittest.mock import patch, MagicMock
from io import StringIO
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from msrest.authentication import CognitiveServicesCredentials

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    @patch('os.getenv')
    def test_main_loads_environment_variables(self, mock_getenv, mock_load_dotenv):
        # Arrange
        mock_getenv.return_value = 'value'
        mock_load_dotenv.return_value = None

        # Act
        main()

        # Assert
        mock_load_dotenv.assert_called_once()
        mock_getenv.assert_called_with('key')

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_main_initializes_computer_vision_client(self, mock_cv_client):
        # Arrange
        mock_creds = MagicMock(spec=CognitiveServicesCredentials)
        mock_cv_client.return_value = mock_creds

        # Act
        main()

        # Assert
        mock_cv_client.assert_called_once_with('endpoint', mock_creds)

    @patch('sys.exit')
    @patch('requests.get')
    def test_main_handles_request_exception(self, mock_requests_get, mock_sys_exit):
        # Arrange
        mock_requests_get.side_effect = requests.exceptions.RequestException('Error')

        # Act & Assert
        with self.assertRaises(SystemExit) as context:
            main()

        # Assert
        mock_requests_get.assert_called_once()
        self.assertEqual(context.exception.code, 1)

if __name__ == '__main__':
    unittest.main()