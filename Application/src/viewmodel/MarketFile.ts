export class BaseEntity {
    id: string;
}

export class ImportedFile extends BaseEntity {
    name?: string;
    fileType: number;
    path?: string;
    url?: string;
    expiredDate: Date;
    isDeleted: boolean;
    isProcessed: boolean;
    errorMessage?: string;
}