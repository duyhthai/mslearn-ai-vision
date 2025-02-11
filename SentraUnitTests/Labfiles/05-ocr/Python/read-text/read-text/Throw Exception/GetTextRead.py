import unittest
from unittest.mock import patch
from io import StringIO
import sys
from your_module import GetTextRead  # Replace 'your_module' with the actual module name

class TestGetTextRead(unittest.TestCase):
    
    @patch('your_module.load_dotenv')
    def test_get_text_read_exception(self, mock_load_dotenv):
        mock_load_dotenv.side_effect = Exception("Failed to load environment variables")
        
        with self.assertRaises(Exception) as context:
            GetTextRead("test_image.jpg")
            
        self.assertIn("Failed to load environment variables", str(context.exception))

if __name__ == '__main__':
    unittest.main()