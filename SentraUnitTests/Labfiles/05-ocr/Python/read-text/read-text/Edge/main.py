import unittest
from unittest.mock import patch
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    def test_main_with_valid_env_vars(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.return_value = None
        os.environ['API_KEY'] = 'valid_key'
        os.environ['SECRET_KEY'] = 'valid_secret'

        # Act
        main()

        # Assert
        mock_load_dotenv.assert_called_once()
        # Add additional assertions based on expected behavior

    @patch('dotenv.load_dotenv')
    def test_main_without_api_key(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.return_value = None
        os.environ['API_KEY'] = ''
        os.environ['SECRET_KEY'] = 'valid_secret'

        # Act
        with self.assertRaises(Exception):
            main()

        # Assert
        mock_load_dotenv.assert_called_once()

    @patch('dotenv.load_dotenv')
    def test_main_without_secret_key(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.return_value = None
        os.environ['API_KEY'] = 'valid_key'
        os.environ['SECRET_KEY'] = ''

        # Act
        with self.assertRaises(Exception):
            main()

        # Assert
        mock_load_dotenv.assert_called_once()

if __name__ == '__main__':
    unittest.main()