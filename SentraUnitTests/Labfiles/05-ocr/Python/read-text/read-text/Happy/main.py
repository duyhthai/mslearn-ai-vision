# Import necessary libraries
import unittest
from unittest.mock import patch
import io
import sys

# Mocking the required functions and classes
@patch('dotenv.load_dotenv')
@patch('os.getenv')
@patch('time.sleep')
@patch('PIL.Image.new')
@patch('PIL.ImageDraw.Draw')
@patch('matplotlib.pyplot.show')

class TestMain(unittest.TestCase):

    def test_main_with_valid_inputs(self, mock_show, mock_draw, mock_new, mock_sleep, mock_getenv, mock_load):
        # Arrange
        mock_getenv.return_value = 'valid_api_key'
        mock_new.return_value = "mock_image"
        
        # Redirect stdout to capture print statements
        old_stdout = sys.stdout
        new_stdout = io.StringIO()
        sys.stdout = new_stdout
        
        # Act
        main()
        
        # Assert
        mock_getenv.assert_called_once_with('API_KEY')
        mock_new.assert_called_once_with('RGB', (100, 100))
        mock_draw.assert_called_once()
        mock_sleep.assert_called_once_with(1)
        mock_show.assert_called_once()
        self.assertIn("Valid API key found", new_stdout.getvalue())
        
        # Cleanup
        sys.stdout = old_stdout

if __name__ == '__main__':
    unittest.main()