# Importing required libraries
import unittest
from unittest.mock import patch
from io import StringIO
import sys

# Mocking the required modules
@patch('dotenv.load_dotenv')
class TestMain(unittest.TestCase):

    def test_load_dotenv_failure(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.side_effect = Exception("Failed to load .env file")
        
        # Redirect stdout to capture print statements
        old_stdout = sys.stdout
        new_stdout = StringIO()
        sys.stdout = new_stdout
        
        # Act
        with self.assertRaises(SystemExit) as context:
            from your_module import main  # Replace 'your_module' with actual module name
        
        # Assert
        self.assertEqual(context.exception.code, 1)
        self.assertIn("Failed to load .env file", new_stdout.getvalue())
        
        # Clean up
        sys.stdout = old_stdout

if __name__ == '__main__':
    unittest.main()