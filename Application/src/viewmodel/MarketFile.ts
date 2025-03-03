export class ImportedFile {
    name?: string;
    fileType: number;
    path?: string;
    url?: string;
    expiredDate: Date;
    isDeleted: boolean;
    isProcessed: boolean;
    errorMessage?: string;
}