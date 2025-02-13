import unittest
from unittest.mock import patch, mock_open
from io import StringIO
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('builtins.open')
    def test_main_with_empty_environment_variables(self, mock_open):
        # Arrange
        mock_open.return_value = mock_open(read_data='').return_value
        os.environ.clear()

        # Act
        with self.assertRaises(SystemExit) as context:
            main()

        # Assert
        self.assertEqual(context.exception.code, 1)
        mock_open.assert_called_once_with('.env', 'r')

    @patch('builtins.open')
    def test_main_with_valid_environment_variables(self, mock_open):
        # Arrange
        mock_open.return_value = mock_open(read_data='API_KEY=valid_key\n').return_value
        os.environ.clear()
        os.environ['API_KEY'] = 'valid_key'

        # Act
        with self.assertRaises(SystemExit) as context:
            main()

        # Assert
        self.assertEqual(context.exception.code, 0)
        mock_open.assert_called_once_with('.env', 'r')

    @patch('builtins.open')
    def test_main_with_missing_api_key(self, mock_open):
        # Arrange
        mock_open.return_value = mock_open(read_data='').return_value
        os.environ.clear()
        os.environ['API_KEY'] = ''

        # Act
        with self.assertRaises(SystemExit) as context:
            main()

        # Assert
        self.assertEqual(context.exception.code, 1)
        mock_open.assert_called_once_with('.env', 'r')

if __name__ == '__main__':
    unittest.main()