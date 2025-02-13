# Import necessary libraries
import unittest
from unittest.mock import patch, Mock
from io import BytesIO

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    def test_main_with_valid_inputs(self, mock_load_dotenv):
        # Arrange
        mock_load_dotenv.return_value = None
        
        # Act
        main()
        
        # Assert
        mock_load_dotenv.assert_called_once()

if __name__ == '__main__':
    unittest.main()