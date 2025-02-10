import unittest
from unittest.mock import patch
from io import StringIO
import sys

class TestGetTextRead(unittest.TestCase):

    @patch('sys.stdout', new_callable=StringIO)
    def test_get_text_read_with_non_image_file(self, mock_stdout):
        # Arrange
        non_image_file = 'non_image.txt'
        
        # Act
        with self.assertRaises(FileNotFoundError) as context:
            GetTextRead(non_image_file)
        
        # Assert
        self.assertIn("No such file or directory", str(context.exception))
    
    @patch('os.path.exists')
    def test_get_text_read_with_invalid_path(self, mock_exists):
        # Arrange
        invalid_path = '/invalid/path/to/image.jpg'
        mock_exists.return_value = False
        
        # Act
        with self.assertRaises(FileNotFoundError) as context:
            GetTextRead(invalid_path)
        
        # Assert
        self.assertIn("No such file or directory", str(context.exception))

    def test_get_text_read_with_empty_image_file(self):
        # Arrange
        empty_image_file = 'empty_image.png'
        open(empty_image_file, 'a').close()
        
        # Act & Assert
        with self.assertRaises(ValueError) as context:
            GetTextRead(empty_image_file)
        
        # Cleanup
        os.remove(empty_image_file)

if __name__ == '__main__':
    unittest.main()