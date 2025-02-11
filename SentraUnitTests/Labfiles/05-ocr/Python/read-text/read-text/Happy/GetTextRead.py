import unittest
from unittest.mock import patch
from io import StringIO
import sys

class TestGetTextRead(unittest.TestCase):
    
    @patch('builtins.print')
    def test_get_text_read(self, mock_print):
        # Arrange
        image_file = 'test_image.jpg'
        
        # Act
        GetTextRead(image_file)
        
        # Assert
        mock_print.assert_called_once_with('\n')

if __name__ == '__main__':
    unittest.main()