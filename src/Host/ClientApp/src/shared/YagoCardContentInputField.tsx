import { Box, TextField } from "@mui/material";

interface YagoCardContentInputFieldProps {
    value: string,
    label: string,
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
    error: string
}

const YagoCardSContentInputField: React.FC<YagoCardContentInputFieldProps> = ({ value, label, handleChange, error }) => {
    return (
        <Box mx={2} textAlign="center">
            <Box mb={2}>
                <TextField
                    fullWidth
                    label={label}
                    value={value}
                    onChange={handleChange}
                    error={!!error}
                    helperText={error}
                    sx={{ mb: 2 }} />
            </Box>
        </Box>
    );
}

export default YagoCardSContentInputField